using System;
using System.Collections.Generic;
using StringEnumGenerator.Tests.GeneratedSourceTests;
using Xunit;

namespace StringEnumGenerator.Tests.RegularEnumTests
{
    public class TryParseTests
    {
        [Fact]
        public void HasTryParseMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString();

            bool result = RegularEnumHelper.TryParseString(input, out var actual);

            Assert.True(result);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HasTryParseWithComparisonMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString().ToLowerInvariant();

            bool result = RegularEnumHelper.TryParseString(input, StringComparison.OrdinalIgnoreCase, out var actual);

            Assert.True(result);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HasTryParseWithComparerMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString().ToLowerInvariant();

            bool result = RegularEnumHelper.TryParseString(input, new OrdinalComparer(), out var actual);

            Assert.True(result);
            Assert.Equal(expected, actual);
        }

        private class OrdinalComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y)
            {
                return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
        }

    }
}
