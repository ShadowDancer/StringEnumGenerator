using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;
using static StringEnumGenerator.Generator.GeneratorConstants;

namespace StringEnumGenerator.Generator.EnumClass
{
    /// <summary>
    /// Generates Parse methods 
    /// </summary>
    internal class EnumParseGenerator
    {
        public EnumParseGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
        {
            EnumContext enumContext1 = enumContext;
            _sourceWriter = sourceWriter;
            _accessibility = SyntaxFacts.GetText(enumContext1.Symbol.DeclaredAccessibility);
            _docWriter = new DocWriter(_sourceWriter);
            _enumName = enumContext1.Symbol.Name;
        }

        private readonly string _enumName;
        private readonly string _accessibility;

        private readonly IndentedTextWriter _sourceWriter;
        private readonly DocWriter _docWriter;

        public void GenerateParse(string mappingName)
        {
            string functionName = GetParseFunctionName(mappingName);
            string tryParseFunctionName = EnumTryParseGenerator.GetTryParseFunctionName(mappingName);

            _docWriter.WriteSummary(DocumentationText.Parse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(Parse.stringToParseParamName, DocumentationText.Parse.StringParam);
            _docWriter.WriteReturn(DocumentationText.Parse.Return(_enumName));
            _docWriter.WriteException(DocumentationText.Parse.ArgumentExceptionFullName, DocumentationText.Parse.ArgumentExceptionText);
            _docWriter.WriteException(DocumentationText.Parse.ArgumentNullExceptionFullName, DocumentationText.Parse.ArgumentNullExceptionText);

            _sourceWriter.WriteLine($"{_accessibility} static {_enumName} {functionName}({stringToParseParam})");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine(ThrowArgumentNullException(Parse.stringToParseParamName));
                CreateIfElse(tryParseFunctionName);
            }
        }

        public void GenerateParseWithStringComparer(string mappingName)
        {
            string functionName = GetParseFunctionName(mappingName);
            string tryParseFunctionName = EnumTryParseGenerator.GetTryParseFunctionName(mappingName);

            _docWriter.WriteSummary(DocumentationText.Parse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(Parse.stringToParseParamName, DocumentationText.Parse.StringParam);
            _docWriter.WriteParamDescription(Parse.comparerParamName, DocumentationText.Parse.ComparerParam);
            _docWriter.WriteReturn(DocumentationText.Parse.Return(_enumName));
            _docWriter.WriteException(DocumentationText.Parse.ArgumentExceptionFullName, DocumentationText.Parse.ArgumentExceptionText);
            _docWriter.WriteException(DocumentationText.Parse.ArgumentNullExceptionFullName, DocumentationText.Parse.ArgumentNullExceptionText);

            _sourceWriter.WriteLine($"{_accessibility} static {_enumName} {functionName}({stringToParseParam}, IEqualityComparer<string> {Parse.comparerParamName})");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine(ThrowArgumentNullException(Parse.stringToParseParamName));
                _sourceWriter.WriteLine(ThrowArgumentNullException(Parse.comparerParamName));
                CreateIfElse(tryParseFunctionName, $"{Parse.comparerParamName}");
            }
        }

        public void GenerateParseWithStringComparison(string mappingName)
        {
            string functionName = GetParseFunctionName(mappingName);
            string tryParseFunctionName = EnumTryParseGenerator.GetTryParseFunctionName(mappingName);

            _docWriter.WriteSummary(DocumentationText.Parse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(Parse.stringToParseParamName, DocumentationText.Parse.StringParam);
            _docWriter.WriteParamDescription(Parse.comparisonParamName, DocumentationText.Parse.ComparisonParam);
            _docWriter.WriteReturn(DocumentationText.Parse.Return(_enumName));
            _docWriter.WriteException(DocumentationText.Parse.ArgumentExceptionFullName, DocumentationText.Parse.ArgumentExceptionText);
            _docWriter.WriteException(DocumentationText.Parse.ArgumentNullExceptionFullName, DocumentationText.Parse.ArgumentNullExceptionText);

            _sourceWriter.WriteLine($"{_accessibility} static {_enumName} {functionName}({stringToParseParam}, StringComparison {Parse.comparisonParamName})");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine(ThrowArgumentNullException(Parse.stringToParseParamName));
                CreateIfElse(tryParseFunctionName, $"{Parse.comparisonParamName}");
            }
        }


        void CreateIfElse(string tryParseFunctionName, string? extraTryParseParameters = null)
        {
            extraTryParseParameters = extraTryParseParameters != null ? ", " + extraTryParseParameters : string.Empty;

            _sourceWriter.WriteLine($"if({tryParseFunctionName}({Parse.stringToParseParamName}{extraTryParseParameters}, out var {Parse.outEnumParamName}))");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine($"return {Parse.outEnumParamName};");
            }
            _sourceWriter.WriteLine("else");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine($"throw new ArgumentException(\"Cannot find requested value \\\"{Parse.stringToParseParamName}\\\"\");");
            }
        }

        private string ThrowArgumentNullException(string argumentName)
        {
            return $"if ({argumentName} == null) {{ throw new System.ArgumentNullException(nameof({argumentName})); }}";
        }

        private string GetParseFunctionName(string mappingName) => "Parse" + mappingName + "String";

        private const string stringToParseParam = "string " + Parse.stringToParseParamName;
    }
}
