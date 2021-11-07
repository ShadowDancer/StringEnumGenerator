using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.EnumClass;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;

namespace StringEnumGenerator.Generator
{
    /// <summary>
    /// Generate enum serializer helper class.
    /// In constructor new class header is written into sourceWriter, when disposed class body will be closed.
    /// </summary>
    public class EnumGenerator
    {
        private readonly EnumContext _enumContext;
        private readonly IndentedTextWriter _sourceWriter;

        private readonly string _accessibility;
        private readonly DocWriter _docWriter;
        private readonly string _enumName;

        internal EnumGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
        {
            _enumContext = enumContext;
            _sourceWriter = sourceWriter;
            _enumName = _enumContext.Symbol.Name;
            _accessibility = SyntaxFacts.GetText(_enumContext.Symbol.DeclaredAccessibility);
            _docWriter = new DocWriter(_sourceWriter);
        }

        public void Generate()
        {
            _docWriter.WriteSummary(DocumentationText.HelperClassSummary(_enumName));
            AddHelperClassHeader(_sourceWriter, _enumContext.Symbol);
            using (_sourceWriter.Scope())
            {
                foreach (var mapping in _enumContext.FieldMappings)
                {
                    AddParseMethods(mapping.Key);
                    AddTryParseMethods(mapping.Key, mapping.Value);
                    AddSerializeExtensionMethod(mapping.Key, mapping.Value);
                }

                AddConstants(_enumContext.FieldMappings);
                AddAllMembersMethod();
            }
        }

        private void AddConstants(ImmutableDictionary<string, ImmutableDictionary<string, string>> fieldMappings)
        {
            var constGenerator = new EnumConstantsGenerator(_enumContext, _sourceWriter);
            constGenerator.GenerateConstClass(fieldMappings);
            _sourceWriter.WriteLine();
        }

        private void AddHelperClassHeader(IndentedTextWriter sourceWriter, INamedTypeSymbol enumSymbol)
        {
            string extensionClassName = enumSymbol.Name + "Helper";

            sourceWriter.WriteLine(_accessibility + " static partial class " + extensionClassName);
        }

        private void AddTryParseMethods(string mappingName, IDictionary<string, string> enumToValue)
        {
            var tryParseGenerator = new EnumTryParseGenerator(_enumContext, _sourceWriter);
            tryParseGenerator.GenerateTryParse(mappingName, enumToValue);
            _sourceWriter.WriteLine();
            tryParseGenerator.GenerateTryParseWithStringComparison(mappingName, enumToValue);
            _sourceWriter.WriteLine();
            tryParseGenerator.GenerateTryParseWithStringComparer(mappingName, enumToValue);
            _sourceWriter.WriteLine();
        }

        private void AddParseMethods(string mappingName)
        {
            var parseGenerator = new EnumParseGenerator(_enumContext, _sourceWriter);
            parseGenerator.GenerateParse(mappingName);
            _sourceWriter.WriteLine();
            parseGenerator.GenerateParseWithStringComparison(mappingName);
            _sourceWriter.WriteLine();
            parseGenerator.GenerateParseWithStringComparer(mappingName);
            _sourceWriter.WriteLine();
        }

        private void AddSerializeExtensionMethod(string mapping, ImmutableDictionary<string, string> enumToValue)
        {
            var toStringGenerator = new EnumToStringGenerator(_enumContext, _sourceWriter);
            toStringGenerator.AddSerializeExtensionMethod(mapping, enumToValue);
            _sourceWriter.WriteLine();
        }

        private void AddAllMembersMethod()
        {
            var enumValues = _enumContext.Fields.Select(n => n.Symbol.Name).ToImmutableArray();
            new EnumAllMembersGenerator(_enumContext, _sourceWriter).GenerateAllMembersFunction(enumValues);
        }
    }
}
