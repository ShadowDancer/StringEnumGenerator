using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;
using static StringEnumGenerator.Generator.GeneratorConstants;

namespace StringEnumGenerator.Generator.EnumClass
{
    /// <summary>
    /// Generates TryParse methods 
    /// </summary>
    internal class EnumTryParseGenerator
    {
        public EnumTryParseGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
        {
            EnumContext enumContext1 = enumContext;
            _sourceWriter = sourceWriter;
            _accessibility = SyntaxFacts.GetText(enumContext1.Symbol.DeclaredAccessibility);
            _docWriter = new DocWriter(_sourceWriter);
            _enumName = enumContext1.Symbol.Name;

            parsedEnumOutParam = $"[MaybeNullWhen(false)] out {_enumName} {TryParse.parsedEnumOutParamName}";

        }

        private readonly string _enumName;
        private readonly string _accessibility;

        private readonly IndentedTextWriter _sourceWriter;
        private readonly DocWriter _docWriter;

        public void GenerateTryParse(string mappingName, IDictionary<string, string> enumToValue)
        {
            string functionName = GetTryParseFunctionName(mappingName);

            // Method using string equality
            _docWriter.WriteSummary(DocumentationText.TryParse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(TryParse.stringToParseParamName, DocumentationText.TryParse.StringParam);
            _docWriter.WriteParamDescription(TryParse.parsedEnumOutParamName, DocumentationText.TryParse.EnumOutParam);
            _docWriter.WriteReturn(DocumentationText.TryParse.Return);

            _sourceWriter.WriteLine($"{_accessibility} static bool {functionName}({stringToParseParam}, {parsedEnumOutParam})");
            using (_sourceWriter.Scope())
            {
                foreach (var kvp in enumToValue)
                {
                    string enumMember = kvp.Key;
                    string enumValue = kvp.Value;

                    _sourceWriter.WriteLine(MakeIfBranchSelectingEnumMember($"{enumValue.ToLiteral()} == {TryParse.stringToParseParamName}", enumMember));
                }

                _sourceWriter.WriteLine(assignNullToOutAndReturnFalseStatement);
            }
        }

        public void GenerateTryParseWithStringComparer(string mappingName, IDictionary<string, string> enumToValue)
        {
            string functionName = GetTryParseFunctionName(mappingName);

            _docWriter.WriteSummary(DocumentationText.TryParse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(TryParse.stringToParseParamName, DocumentationText.TryParse.StringParam);
            _docWriter.WriteParamDescription(TryParse.comparerParamName, DocumentationText.TryParse.ComparerParam);
            _docWriter.WriteParamDescription(TryParse.parsedEnumOutParamName, DocumentationText.TryParse.EnumOutParam);
            _docWriter.WriteReturn(DocumentationText.TryParse.Return);

            _sourceWriter.WriteLine($"{_accessibility} static bool {functionName}({stringToParseParam}, IEqualityComparer<string> {TryParse.comparerParamName}, {parsedEnumOutParam})");
            using (_sourceWriter.Scope())
            {
                foreach (var kvp in enumToValue)
                {
                    string enumMember = kvp.Key;
                    string enumValue = kvp.Value;

                    _sourceWriter.WriteLine(ThrowArgumentNullException(TryParse.comparerParamName));
                    _sourceWriter.WriteLine(MakeIfBranchSelectingEnumMember($"{TryParse.comparerParamName}.Equals({enumValue.ToLiteral()}, {TryParse.stringToParseParamName})", enumMember));
                }

                _sourceWriter.WriteLine(assignNullToOutAndReturnFalseStatement);
            }
        }

        public void GenerateTryParseWithStringComparison(string mappingName, IDictionary<string, string> enumToValue)
        {
            string functionName = GetTryParseFunctionName(mappingName);

            _docWriter.WriteSummary(DocumentationText.TryParse.Summary(_enumName, mappingName));
            _docWriter.WriteParamDescription(TryParse.stringToParseParamName, DocumentationText.TryParse.StringParam);
            _docWriter.WriteParamDescription(TryParse.comparisonParamName, DocumentationText.TryParse.ComparisonParam);
            _docWriter.WriteParamDescription(TryParse.parsedEnumOutParamName, DocumentationText.TryParse.EnumOutParam);
            _docWriter.WriteReturn(DocumentationText.TryParse.Return);

            _sourceWriter.WriteLine($"{_accessibility} static bool {functionName}({stringToParseParam}, StringComparison {TryParse.comparisonParamName}, {parsedEnumOutParam})");
            using (_sourceWriter.Scope())
            {
                foreach (var kvp in enumToValue)
                {
                    string enumMember = kvp.Key;
                    string enumValue = kvp.Value;

                    _sourceWriter.WriteLine(MakeIfBranchSelectingEnumMember($"string.Equals({enumValue.ToLiteral()}, {TryParse.stringToParseParamName}, {TryParse.comparisonParamName})", enumMember));
                }

                _sourceWriter.WriteLine(assignNullToOutAndReturnFalseStatement);
            }
        }

        private string ThrowArgumentNullException(string argumentName)
        {
            return $"if ({argumentName} == null) {{ throw new System.ArgumentNullException(nameof({argumentName})); }}";
        }

        string MakeIfBranchSelectingEnumMember(string ifCondition, string resultEnumMember)
        {
            return $"if({ifCondition}) {{ {TryParse.parsedEnumOutParamName} = {_enumName}.{resultEnumMember}; return true; }}";
        }

        public static string GetTryParseFunctionName(string mappingName) => "TryParse" + mappingName + "String";

        private readonly string parsedEnumOutParam;

        const string stringToParseParam = "string " + TryParse.stringToParseParamName;

        const string assignNullToOutAndReturnFalseStatement = TryParse.parsedEnumOutParamName + " = default; return false;";
    }
}
