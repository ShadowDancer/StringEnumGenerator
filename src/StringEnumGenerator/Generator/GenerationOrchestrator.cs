using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StringEnumGenerator.Generator.Model;

namespace StringEnumGenerator.Generator
{
    /// <summary>
    /// Discovers enums and passes them to <see cref="ClassGenerator"/>
    /// </summary>
    public class GenerationOrchestrator
    {
        private readonly GeneratorExecutionContext _context;
        private readonly EnumInspector _enumInspector;
        public GenerationOrchestrator(GeneratorExecutionContext context)
        {
            _context = context;
            _enumInspector = new EnumInspector(_context);
        }

        public void Generate()
        {
            var compilation = _context.Compilation;

            var allNodes =
                compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            var allClasses = allNodes
                .OfType<EnumDeclarationSyntax>();

            var generatedSource = allClasses
                .Select(classDeclaration => TryGenerateEnumHelper(compilation, classDeclaration))
                .Where(n => n != null)
                .Select(n => n!.Value)
                .ToImmutableArray();

            foreach ((var className, var source) in generatedSource) _context.AddSource(className, source);
        }

        private (string className, string source)? TryGenerateEnumHelper(Compilation compilation,
            EnumDeclarationSyntax enumDeclaration)
        {
            var semanticModel = compilation.GetSemanticModel(enumDeclaration.SyntaxTree);
            var enumSymbol = semanticModel.GetDeclaredSymbol(enumDeclaration);

            if (enumSymbol == null)
            {
                return null;
            }

            var context = new EnumContext(enumSymbol, enumDeclaration, semanticModel);

            if (!_enumInspector.IsStringEnum(context, semanticModel))
            {
                return null;
            }

            var source = ClassGenerator.GenerateSource(context, semanticModel);

            var fullName = enumSymbol.ContainingNamespace.ToDisplayString() + "." + enumSymbol.Name;
            return (fullName, source);
        }
    }
}
