using System;
using StringEnumGenerator.Attributes;

// Missing XML comment for publicly visible type
#pragma warning disable CS1519
#pragma warning disable CS1591

namespace StringEnumGenerator.Sample
{
    public enum NestedEnum
    {
        Carrot = 1
    }

    public enum Food
    {
        [StringEnum(Display = "Orange Carrot", Value = "orange_carrot")]
        Carrot = 1,

        [StringEnum(Display = "Brown Bread", Value = "fresh_bread")]
        Bread = 2,

        [StringEnum(Display = "Red Apple", Value = "red_apple")]
        Apple = 3,

        [StringEnum(Display = "Pork", Value = "pork")]
        Pork = 4,

        [StringEnum(Display = "Fresh Lettuce", Value = "lettuce")]
        Lettuce = 5,
    }

    static class Program
    {
        static void Main()
        {
            string[] headers = { "EnumName", "Display", "Value", "Numeric" };
            WritePadded(headers);
            foreach (Food enumValue in Enum.GetValues(typeof(Food)))
            {
                // To*String methods are highlighted by intellisense because analyzer project is referenced via ProjectReference
                // does not happen when using nuget package
                string[] values =
                {
                    enumValue.ToString(), enumValue.ToDisplayString(), enumValue.ToValueString(),
                    ((int)enumValue).ToString()
                };
                WritePadded(values);
            }
        }

        /// <summary>
        /// <see cref="System.Collections.Generic.IEqualityComparer{X}"/>
        /// </summary>
        /// <param name="dataArray"></param>
        private static void WritePadded(string[] dataArray)
        {
            foreach (string data in dataArray)
            {
                Console.Write(data.PadRight(20));
            }

            Console.WriteLine();
        }
    }
}
