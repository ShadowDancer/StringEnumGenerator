
using Microsoft.CodeAnalysis;
using StringEnumGenerator.Attributes;

namespace StringEnumGenerator
{
    public static class Diagnostics
    {
        private static readonly DiagnosticDescriptor EnumInPartialClassDescriptor = new(
            "SEG0001",
            "StringEnum in nested class",
            "Cannot generate code for {0} becuase it is nested inside another type. To fix this error either move enum to top level, or remove " + nameof(StringEnumAttribute) + " from its members.",
            "StringEnumGenerator",
            DiagnosticSeverity.Error,
            true);
        public static Diagnostic EnumInPartialClass(Location? location, string enumName)
        {
            return Diagnostic.Create(EnumInPartialClassDescriptor, location, enumName);
        }

        private static readonly DiagnosticDescriptor MissingPropertyAnnotationDescriptor = new(
            "SEG0002",
            "Missing " + nameof(StringEnumAttribute) + " on enum member.",
            "Mark {0}.{1} with " + nameof(StringEnumAttribute) + " and provide {2} value, or remove all " + nameof(StringEnumAttribute) + "s from this enum. This is necessary to generate Parse and ToString methods.",
            "StringEnumGenerator",
            DiagnosticSeverity.Error,
            true);
        public static Diagnostic MissingPropertyAnnotation(Location? location, string enumName, string fieldName, string mappingName)
        {
            return Diagnostic.Create(MissingPropertyAnnotationDescriptor, location, enumName, fieldName, mappingName);
        }

        private static readonly DiagnosticDescriptor DuplicateEnumStringValueDescriptor = new(
            "SEG0003",
            "Duplicated StringEnum value",
            "Other member of {0} is already marked with \"{1}\" for {2}. StringEnum values must be unique.",
            "StringEnumGenerator",
            DiagnosticSeverity.Error,
            true);
        public static Diagnostic DuplicatedEnumStringValue(Location? location, string enumName, string value, string mapping)
        {
            return Diagnostic.Create(DuplicateEnumStringValueDescriptor, location, enumName, value, mapping);
        }

    }
}
