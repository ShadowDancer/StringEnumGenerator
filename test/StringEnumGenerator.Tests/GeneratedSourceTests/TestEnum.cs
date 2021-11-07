using StringEnumGenerator.Attributes;

namespace StringEnumGenerator.Tests.GeneratedSourceTests;

public enum TestEnum
{
    [StringEnum(Display = TestEnumConstants.UpperCasePropDisplay, Value = TestEnumConstants.UpperCasePropValue)]
    UpperCaseProp = 1,

    [StringEnum(Display = TestEnumConstants.CamelCasePropDisplay, Value = TestEnumConstants.CamelCasePropValue)]
    CamelCaseProp = 2
}

internal class TestEnumConstants
{
    public const string UpperCasePropDisplay = "UPPERCASEPROPDISPLAY";
    public const string UpperCasePropValue = "UPPERCASEPROPVALUE";

    public const string CamelCasePropDisplay = "CamelCasePropDisplay";
    public const string CamelCasePropValue = "CamelCasePropValue";
}
