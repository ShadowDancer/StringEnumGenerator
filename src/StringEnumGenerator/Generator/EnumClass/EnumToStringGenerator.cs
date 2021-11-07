using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;

namespace StringEnumGenerator.Generator.EnumClass
{
    internal class EnumToStringGenerator
    {
        public EnumToStringGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
        {
            _enumContext = enumContext;
            _sourceWriter = sourceWriter;
            _accessibility = SyntaxFacts.GetText(_enumContext.Symbol.DeclaredAccessibility);
            _docWriter = new DocWriter(_sourceWriter);
            _enumName = _enumContext.Symbol.Name;
        }

        private readonly string _enumName;
        private readonly EnumContext _enumContext;
        private readonly string _accessibility;

        private readonly IndentedTextWriter _sourceWriter;
        private readonly DocWriter _docWriter;

        public void AddSerializeExtensionMethod(string mappingName, IDictionary<string, string> enumToValue)
        {
            string functionName = GetSerializeFunctionName(mappingName);
            const string enumValueParameter = "enumValue";

            _docWriter.WriteSummary(DocumentationText.Serialize.Summary(_enumName, mappingName));
            _docWriter.WriteReturn(DocumentationText.Serialize.Return(_enumName, mappingName));

            _sourceWriter.WriteLine($"{_accessibility} static string {functionName}(this {_enumContext.Symbol.Name} {enumValueParameter})");
            using (_sourceWriter.Scope())
            {
                _sourceWriter.WriteLine($"return {enumValueParameter} switch");
                using (_sourceWriter.ExpressionScope())
                {
                    foreach (var kvp in enumToValue)
                    {
                        string enumMember = kvp.Key;
                        string enumValue = kvp.Value;

                        _sourceWriter.WriteLine($"{_enumName}.{enumMember} => {enumValue.ToLiteral()},");
                    }

                    _sourceWriter.WriteLine("var unknown => throw new System.NotSupportedException(\"Unknown enum member\" + unknown)");
                }
            }
            _sourceWriter.WriteLine();
        }

        private bool IsFastMapping(string mappingName)
        {
            return mappingName == GeneratorConstants.FieldNameMapping;
        }

        private string GetSerializeFunctionName(string mappingName)
        {
            if(IsFastMapping(mappingName))
            {
                return "ToStringFast";
            }
            return "To" + mappingName + "String";
        }
    }
}
