namespace StringEnumGenerator.Generator
{
    internal static class GeneratorConstants
    {
        /// <summary>
        /// This represents artificial mapping added to the system which represents enum field name. 
        /// </summary>
        internal const string FieldNameMapping = "";
        internal const string FieldNameToStringMapping = "";

        internal static class TryParse
        {
            public const string stringToParseParamName = "stringToParse";

            public const string comparerParamName = "stringComparer";

            public const string comparisonParamName = "stringComparison";

            public const string parsedEnumOutParamName = "parsedEnum";
        }

        internal static class Parse
        {
            public const string stringToParseParamName = "stringToParse";

            public const string comparerParamName = "stringComparer";

            public const string comparisonParamName = "stringComparison";

            public const string outEnumParamName = "parsedEnum";
        }
    }
}
