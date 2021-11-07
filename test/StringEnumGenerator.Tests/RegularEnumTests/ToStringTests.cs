using StringEnumGenerator.Tests.GeneratedSourceTests;
using Xunit;

namespace StringEnumGenerator.Tests.RegularEnumTests
{
    public class ToStringTests
    {
        [Fact]
        public void HasFastToStringMethod()
        {
            string actual = RegularEnum.Member1.ToStringFast();

            Assert.Equal(RegularEnum.Member1.ToString(), actual);
        }
    }
}
