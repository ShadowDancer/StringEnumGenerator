using System;
using System.Collections.Generic;
using StringEnumGenerator.Tests.GeneratedSourceTests;
using Xunit;

namespace StringEnumGenerator.Tests.RegularEnumTests
{
    public class ParseTests
    {
        [Fact]
        public void HasParseMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString();

            var actual = RegularEnumHelper.ParseString(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HasParseWithComparisonMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString().ToLowerInvariant();

            var actual = RegularEnumHelper.ParseString(input, StringComparison.OrdinalIgnoreCase);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HasParseWithComparerMethod()
        {
            var expected = RegularEnum.Member1;
            string input = expected.ToString().ToLowerInvariant();

            var actual = RegularEnumHelper.ParseString(input, new OrdinalComparer());

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
