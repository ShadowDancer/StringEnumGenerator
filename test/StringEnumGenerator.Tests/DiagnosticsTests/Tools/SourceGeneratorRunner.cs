extern alias StringGeneratorAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Reflection;
using Xunit;
using System.Collections.Immutable;

namespace StringEnumGenerator.Tests.DiagnosticsTests.Tools
{
    public class SourceGeneratorRunner
    {
        public static string ProgramPrefix = @"
namespace MyCode
{
    public static class Program
    {
        public static void Main(){ }
    }
}
            ";

        public static GeneratorRunResult RunCodeGeneratorForSingleResult(string input, out Compilation outputCompilation, out ImmutableArray<Diagnostic> diagnostics)
        {
            var inputCompilation = CreateCompilation(ProgramPrefix + input);
            Assert.Single(inputCompilation.SyntaxTrees); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
            var inputDiagnostics = inputCompilation.GetDiagnostics();
            Assert.Empty(inputDiagnostics); // verify the compilation with the added source has no diagnostics


            // directly create an instance of the generator
            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
            SourceGenerator generator = new();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the generation pass
            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out outputCompilation, out diagnostics);

            // We can now assert things about the resulting compilation:
            Assert.Empty(diagnostics); // there were no diagnostics created by the generators
            Assert.Equal(2, outputCompilation.SyntaxTrees.Count()); // we have two syntax trees, the original 'user' provided one, and the one added by the generator

            var outputDiagnostics = outputCompilation.GetDiagnostics();
            Assert.Empty(outputDiagnostics); // verify the compilation with the added source has no diagnostics

            var runResult = driver.GetRunResult();
            Assert.True(runResult.GeneratedTrees.Length == 1);
            Assert.Empty(runResult.Diagnostics);

            var results = runResult.Results.Where(n => n.Generator == generator).ToArray();
            // This method supports just single compilation
            Assert.Single(results);

            var result = results[0];
            Assert.True(result.GeneratedSources.Length > 0);
            return results[0];
        }

        private static Compilation CreateCompilation(string source)
        {
            return CSharpCompilation.Create("compilation",
                           new[] { CSharpSyntaxTree.ParseText(source) },
                           new[] {
                    MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(ImmutableArray<>).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(StringGeneratorAttributes.StringEnumGenerator.Attributes.StringEnumAttribute).GetTypeInfo().Assembly.Location)
                           },
                           new CSharpCompilationOptions(OutputKind.ConsoleApplication ));
        }
    }
}
