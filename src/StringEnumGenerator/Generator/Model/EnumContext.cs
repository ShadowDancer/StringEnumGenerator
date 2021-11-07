using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StringEnumGenerator.Attributes;

namespace StringEnumGenerator.Generator.Model
{
    internal record EnumContext
    {
        public EnumContext(INamedTypeSymbol symbol, EnumDeclarationSyntax syntax, SemanticModel semanticModel)
        {
            var stringEnumAttribute = semanticModel.Compilation.GetTypeByMetadataName(typeof(StringEnumAttribute).FullName)!;
            Symbol = symbol;
            Syntax = syntax;
            Fields = Symbol.GetMembers()
                .Where(n => n is IFieldSymbol)
                .Cast<IFieldSymbol>()
                .Select(n => new FieldContext(n, stringEnumAttribute))
                .ToImmutableArray();

            var fieldMappings = new Dictionary<string, Dictionary<string, string>>();

            foreach (var field in Fields)
            {
                foreach (var mapping in field.Mappings)
                {
                    if (!fieldMappings.ContainsKey(mapping.Key))
                    {
                        fieldMappings.Add(mapping.Key, new Dictionary<string, string>());
                    }

                    fieldMappings[mapping.Key].Add(field.Symbol.Name, mapping.Value);
                }
            }

            FieldMappings = fieldMappings.ToImmutableDictionary(n => n.Key, n => n.Value.ToImmutableDictionary());
        }

        public INamedTypeSymbol Symbol { get; }
        public EnumDeclarationSyntax Syntax { get; }
        public ImmutableArray<FieldContext> Fields { get; }

        public string Name => Symbol.Name;

        /// <summary>
        /// Aggregated mappings of all fields. Contains values in form:
        /// { "MappingName": { "FieldName": "MappingValue" } }
        /// i.e. { "Display": { "EnumField1": "Enum field 1 display value" } }
        /// 
        /// GeneratorConstants.FieldNameMapping (empty string) represents enum field name.
        /// </summary>
        public ImmutableDictionary<string, ImmutableDictionary<string, string>> FieldMappings { get; }

    }
}
