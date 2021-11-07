using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using StringEnumGenerator.Generator.Model;
using StringEnumGenerator.Generator.Utils;

namespace StringEnumGenerator.Generator
{
    /// <summary>
    /// Checks if Helper class can be generated for Enum
    /// </summary>
    internal class EnumInspector
    {

        private readonly GeneratorExecutionContext _context;

        public EnumInspector(GeneratorExecutionContext context)
        {
            _context = context;
        }

        public bool IsStringEnum(EnumContext enumContext, SemanticModel semanticModel)
        {
            bool isStringEnum = HasAnyStringEnum(enumContext);

            if (IsNestedEnum(enumContext, isStringEnum))
            {
                return false;
            }

            bool failed = false;
            failed = failed || HasAnyMissingStringValues(enumContext);

            failed = failed || HasAnyDuplicateValues(enumContext);
            if (failed)
            {
                return false;
            }

            // Try to generate faster non-reflection based methods even for regular enums
            return true;
        }


        private bool HasAnyMissingStringValues(EnumContext enumContext)
        {
            bool failed = false;
                int fieldsCount = enumContext.Fields.Length;

                foreach (var mapping in enumContext.FieldMappings)
                {
                    if (mapping.Value.Count < fieldsCount)
                    {
                        string[] fieldsWithDeclaredValue = mapping.Value.Keys.ToArray();
                        var fieldsWithoutDeclaredValue = enumContext.Fields
                            .Where(n => !fieldsWithDeclaredValue.Contains(n.Symbol.Name));

                        foreach(var field in fieldsWithoutDeclaredValue)
                        {
                            var location = field.Symbol.Locations.FirstOrDefault();
                            var diagnostic = Diagnostics.MissingPropertyAnnotation(location, enumContext.Name, field.Name, mapping.Key);
                            _context.ReportDiagnostic(diagnostic);
                            failed = true;
                        }
                    }
            }

            return failed;
        }

        private bool HasAnyDuplicateValues(EnumContext enumContext)
        {
            bool hasDuplicates = false;
            foreach (var mapping in enumContext.FieldMappings)
            {
                List<string> existingElements = new();
                List<(string mappingKey, string mappingValue)> duplicates = new();
                foreach (var kvp in mapping.Value)
                {
                    string value = kvp.Value;
                    if(existingElements.Any(n => string.Equals(n, value, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        string mappingKey = mapping.Key;
                        duplicates.Add((mappingKey, value));
                    }
                    else
                    {
                        existingElements.Add(value);
                    }
                }

                if (duplicates.Any())
                {
                    foreach(var field in enumContext.Fields)
                    {
                        foreach(var attribute in field.Attributes)
                        {
                            foreach(var attributeMapping in attribute.Mappings)
                            {
                                foreach(var duplicate in duplicates)
                                {
                                    if(attributeMapping.Key == duplicate.mappingKey && attributeMapping.Value == duplicate.mappingValue)
                                    {
                                        var location = field.Symbol.Locations.FirstOrDefault();
                                        var diagnostic = Diagnostics.DuplicatedEnumStringValue(location, enumContext.Name, duplicate.mappingValue, attributeMapping.Key);
                                        _context.ReportDiagnostic(diagnostic);
                                        hasDuplicates = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return hasDuplicates;
        }

        private bool IsNestedEnum(EnumContext enumContext, bool isStringEnum)
        {
            if (enumContext.Symbol.GetContainingTypes().Any())
            {
                if (isStringEnum)
                {
                    // Report diagnostics only for enums with string enum attribute, ignore regular enums
                    string enumName = enumContext.Syntax.Identifier.Text;
                    var diagnostic = Diagnostics.EnumInPartialClass(enumContext.Syntax.GetLocation(), enumName);
                    _context.ReportDiagnostic(diagnostic);
                }
                return true;
            }

            return false;
        }

        private static bool HasAnyStringEnum(EnumContext enumContext)
        {
            if (enumContext.FieldMappings.Count > 1)
            {
                // There is always at least 1 mapping for enum name (so called "fast methods"),
                // so if we have 2 there is at least ne string enum member.
                return true;
            }

            return false;
        }
    }
}
