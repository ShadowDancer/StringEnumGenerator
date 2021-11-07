using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace StringEnumGenerator.Generator.Model
{
    internal record FieldAttributeContext
    {
        public FieldAttributeContext(AttributeData attributeData, INamedTypeSymbol stringEnumAttribute)
        {
            AttributeData = attributeData;

            IsStringEnumAttribute = SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass, stringEnumAttribute);

            var mappings = new Dictionary<string, string>();
            ParseConstructorArguments(AttributeData, mappings);
            Mappings = mappings.ToImmutableDictionary();
        }

        public AttributeData AttributeData { get; }
        public bool IsStringEnumAttribute { get; }

        /// <summary>
        /// Mapping of constructor arguments, i.e. if constructor contains Display="DisplayVal" this dict will contain { "Display": "DisplayVal" } pair.
        /// </summary>
        public ImmutableDictionary<string, string> Mappings { get; }

        private void ParseConstructorArguments(AttributeData attributeData, Dictionary<string, string> mappings)
        {
			if(!IsStringEnumAttribute)
			{
				return;
			}
			
            foreach (var arg in attributeData.NamedArguments)
            {
                if (arg.Value.IsNull)
                {
                    continue;
                }

                string? stringValue = arg.Value.Value?.ToString();

                if(stringValue == null)
                {
                    continue;
                }

                mappings[arg.Key] = stringValue;
            }
        }
    }
}
