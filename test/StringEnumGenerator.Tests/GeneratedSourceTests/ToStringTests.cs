using System;
using Xunit;

namespace StringEnumGenerator.Tests.GeneratedSourceTests
{
    public class ParseTests
    {
        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropDisplay)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropDisplay)]
        public void Should_ParseDisplay_When_ExactMatch(TestEnum expected, string input)
        {
            TestEnum actual = TestEnumHelper.ParseDisplayString(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropValue)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropValue)]
        public void Should_ParseValue_When_ExactMatch(TestEnum expected, string input)
        {
            TestEnum actual = TestEnumHelper.ParseValueString(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnumConstants.CamelCasePropDisplay)]
        [InlineData(TestEnumConstants.UpperCasePropDisplay)]
        public void Should_FailToParseValue_When_GivenDisplayStrings(string input)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = TestEnumHelper.ParseValueString(input);
            });
        }

        [Theory]
        [InlineData(TestEnum.CamelCaseProp)]
        [InlineData(TestEnum.UpperCaseProp)]
        public void Should_FailToParseValue_When_GivenEnumMembers(TestEnum input)
        {
            string inputString = input.ToString();
            Assert.Throws<ArgumentException>(() =>
            {
                _ = TestEnumHelper.ParseValueString(inputString);
            });
        }

        [Fact]
        public void Should_Throw_When_GivenInvalidString()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = TestEnumHelper.ParseValueString("invalid entry");
            });
        }

        [Fact]
        public void Should_Throw_When_GivenNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _ = TestEnumHelper.ParseValueString(null!);
            });
        }
    }
}
