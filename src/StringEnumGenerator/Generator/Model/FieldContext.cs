using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace StringEnumGenerator.Generator.Model
{
    internal record FieldContext
    {
        public FieldContext(IFieldSymbol declaration, INamedTypeSymbol stringEnumAttribute)
        {
            Symbol = declaration;
            Attributes = declaration.GetAttributes()
                .Select(n => new FieldAttributeContext(n, stringEnumAttribute))
                .ToImmutableArray();

            var mappings = new Dictionary<string, string>
            {
                { GeneratorConstants.FieldNameMapping, declaration.Name }
            };

            foreach (var attribute in Attributes)
            {
                foreach (var kvp in attribute.Mappings)
                {
                    mappings.Add(kvp.Key, kvp.Value);
                }
            }

            Mappings = mappings.ToImmutableDictionary();
        }

        public IFieldSymbol Symbol { get; }

        public ImmutableArray<FieldAttributeContext> Attributes { get; }

        /// <summary>
        /// Aggregated mapping for all attributes in form { "MapName": "MapValue" } i.e. {"Display": "Display value"}
        /// GeneratorConstants.FieldNameMapping (empty string) represents enum field name.
        /// </summary>
        public ImmutableDictionary<string, string> Mappings { get; }
        public string Name => Symbol.Name;
    }
}
