using System;
using Xunit;

namespace StringEnumGenerator.Tests.GeneratedSourceTests
{
    public class ToStringTests
    {
        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropDisplay)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropDisplay)]
        public void Should_SerializeDisplayCorrectly(TestEnum enumValue, string expected)
        {
            var actual = enumValue.ToDisplayString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropValue)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropValue)]
        public void Should_SerializeValueCorrectly(TestEnum enumValue, string expected)
        {
            var actual = enumValue.ToValueString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Throw_When_InvalidValue()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                var e = (TestEnum)256;
                _ = e.ToDisplayString();
            });
        }
    }
}
