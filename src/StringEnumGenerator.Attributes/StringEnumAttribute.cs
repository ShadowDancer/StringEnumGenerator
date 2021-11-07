using System;

namespace StringEnumGenerator.Attributes
{
    /// <summary>
    /// When enum member is annotated with this attribute whole enum becomes StringEnum.
    /// StringEnum must have all members annotated with <see cref="StringEnumAttribute"/>.
    /// 
    /// Each <see cref="StringEnumAttribute"/> must have either <see cref="Display"/> or <see cref="Value"/> parameter.
    /// 
    /// If any member has <see cref="Display"/> parameter, all other members must have <see cref="Display"/> parameter,
    /// and `EnumName`.ToDisplay(), `EnumName`.FromDisplay(EnumName enum) extension methods will be generated, which 
    /// allow converting 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StringEnumAttribute : Attribute
    {
        public string? Display { get; set; }

        public string? Value { get; set; }
    }
}
