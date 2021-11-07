using Xunit;

namespace StringEnumGenerator.Tests.GeneratedSourceTests
{
    public class TryParseTests
    {
        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropDisplay)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropDisplay)]
        public void Should_ParseDisplay_When_ExactMatch(TestEnum expected, string input)
        {
            var result = TestEnumHelper.TryParseDisplayString(input, out TestEnum actual);

            Assert.True(result);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.CamelCaseProp, TestEnumConstants.CamelCasePropValue)]
        [InlineData(TestEnum.UpperCaseProp, TestEnumConstants.UpperCasePropValue)]
        public void Should_ParseValue_When_ExactMatch(TestEnum expected, string input)
        {
            var result = TestEnumHelper.TryParseValueString(input, out TestEnum actual);

            Assert.True(result);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(TestEnum.CamelCaseProp)]
        [InlineData(TestEnum.UpperCaseProp)]
        public void Should_FailToParseValue_When_GivenEnumMemberNames(TestEnum input)
        {
            var result = TestEnumHelper.TryParseValueString(input.ToString(), out _);

            Assert.False(result);
        }

        [Theory]
        [InlineData(TestEnumConstants.CamelCasePropDisplay)]
        [InlineData(TestEnumConstants.UpperCasePropDisplay)]
        public void Should_FailToParseValue_When_GivenDisplayStrings(string input)
        {
            var result = TestEnumHelper.TryParseValueString(input, out _);

            Assert.False(result);
        }

        [Fact]
        public void Should_Fail_When_GivenInvalidValue()
        {
            var result = TestEnumHelper.TryParseDisplayString("invalid entry", out TestEnum actual);

            Assert.False(result);
            Assert.Equal(default, actual);
        }

        [Fact]
        public void Should_Fail_When_GivenNull()
        {
            var result = TestEnumHelper.TryParseDisplayString(null!, out TestEnum actual);

            Assert.False(result);
            Assert.Equal(default, actual);
        }
    }
}
