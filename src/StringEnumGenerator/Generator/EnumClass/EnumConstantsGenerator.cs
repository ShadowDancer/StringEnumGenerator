using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;
using static StringEnumGenerator.Generator.DocumentationText;

namespace StringEnumGenerator.Generator.EnumClass
{
    /// <summary>
    /// Generates static class witch constant values for each mapping 
    /// </summary>
    internal class EnumConstantsGenerator
    {
        public EnumConstantsGenerator(EnumContext enumContext, IndentedTextWriter sourceWriter)
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

        private const string ConstantsClassName = "Consts";
        
        
        public void GenerateConstClass(ImmutableDictionary<string, ImmutableDictionary<string, string>> mappings)
        {
            var mappingsToProcess = mappings
                .Where(n => n.Key != GeneratorConstants.FieldNameMapping)
                .ToList();

            if (!mappingsToProcess.Any())
            {
                return;
            }
            
            _docWriter.WriteSummary(Constants.Summary(_enumName));
            _sourceWriter.WriteLine($"{_accessibility} static class {ConstantsClassName}");
            using (_sourceWriter.Scope())
            {
                foreach (var mapping in mappingsToProcess)
                {
                    _docWriter.WriteSummary(Constants.MappingClassSummary(_enumName, mapping.Key));
                    _sourceWriter.WriteLine($"{_accessibility} static class {mapping.Key}");
                    using (_sourceWriter.Scope())
                    {
                        foreach (var kvp in mapping.Value)
                        {
                            _docWriter.WriteSummary(Constants.MappingMemberSummary(_enumName, mapping.Key, kvp.Key));
                            _sourceWriter.WriteLine($"public const string {kvp.Key} = {kvp.Value.ToLiteral()};");
                        }
                    }
                }
            }
        }
    }
}
