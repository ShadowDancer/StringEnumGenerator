using StringEnumGenerator.Attributes;

namespace StringEnumGenerator.Generator
{
    internal static class DocumentationText
    {
        internal static class TryParse
        {
            internal static string Summary(string enumName, string mappingName)
            {
                if (mappingName == GeneratorConstants.FieldNameMapping)
                {
                    return "Converts the string representation to " + DocWriter.Cref(enumName) +
                           " instance. A return value indicates whether the operation succeeded.";
                }

                return "Converts the " + mappingName + " string representation to " + DocWriter.Cref(enumName) +
                       " instance. A return value indicates whether the operation succeeded.";
            }

            internal const string StringParam = "A string containing a enum to convert.";

            internal const string EnumOutParam =
                "When this method returns, contains the enum value equivalent of the string, if the conversion succeeded, or default enum value if the conversion failed. The conversion fails if the string is not equal to any of the enum members.";

            internal static string Return =
                "<see langword=\"keyword\">true</see> if s was converted successfully; otherwise, <see langword=\"keyword\">false</see>.";

            public const string ComparerParam =
                "<see cref=\"System.Collections.Generic.IEqualityComparer{T}\"/> to use when comparing strings to enum identifiers.";

            public const string ComparisonParam =
                "One of the enumeration values that specifies the rules to use when compraring strings to enum identifiers";
        }

        internal static string HelperClassSummary(string enumName)
        {
            return
                $"Contains helper methods for converting <see cref=\"{enumName}\"/> to and from string representations, and high performance enum methods.";
        }

        internal static class Parse
        {
            internal static string Summary(string enumName, string mappingName)
            {
                if (mappingName == string.Empty)
                {
                    return "Converts the string representation to " + DocWriter.Cref(enumName) +
                           " instance. A return value indicates whether the operation succeeded.";
                }

                return "Converts the " + mappingName + " string representation to " + DocWriter.Cref(enumName) +
                       " instance. A return value indicates whether the operation succeeded.";
            }

            internal const string StringParam = "A string containing a enum to convert.";

            public const string ComparerParam =
                "<see cref=\"System.Collections.Generic.IEqualityComparer{T}\"/> to use when comparing strings to enum identifiers.";

            public const string ComparisonParam =
                "One of the enumeration values that specifies the rules to use when compraring strings to enum identifiers";

            internal static string Return(string enumName)
            {
                return DocWriter.Cref(enumName) + "instance parsed from " +
                       GeneratorConstants.Parse.stringToParseParamName;
            }

            internal const string ArgumentExceptionFullName = "System.ArgumentException";

            internal const string ArgumentExceptionText = "Provided string is not equal to any enum member.";

            internal const string ArgumentNullExceptionFullName = "System.ArgumentNullException";

            internal const string ArgumentNullExceptionText = "Provided argument is null.";
        }

        internal static class Serialize
        {
            internal static string Summary(string enumName, string mappingName)
            {
                return "Converts the " + DocWriter.Cref(enumName) + " to " + mappingName + " string representation.";
            }

            internal static string Return(string enumName, string mappingName)
            {
                return mappingName + " string representation of " + DocWriter.Cref(enumName) + ".";
            }
        }

        internal static class AllMembers
        {
            public static string Summary(string enumName)
            {
                return "Array with all members of " + enumName;
            }
        }

        internal static class Constants
        {
            public static string Summary(string enumName)
            {
                return
                    $"This class is generated from {DocWriter.Cref(enumName)} and contains values of {StringEnumAttributeCref} exposed as string constants";
            }

            public static string MappingClassSummary(string enumName, string mappingName)
            {
                return $"{mappingName} string values of {DocWriter.Cref(enumName)} exposed as string constants";
            }

            public static string MappingMemberSummary(string enumName, string mappingKey, string enumMember)
            {
                return
                    $"{mappingKey} string value for {DocWriter.Cref(enumName + "." + enumMember)} member of {DocWriter.Cref(enumName)}. This constant is generated from value passed to {StringEnumAttributeCref}";
            }

            private static readonly string StringEnumAttributeCref = DocWriter.Cref(typeof(StringEnumAttribute).FullName);
        }
    }
}
