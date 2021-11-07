using Xunit;

namespace StringEnumGenerator.Tests.GeneratedSourceTests
{
    public class ConstantTests 
    {
        [Fact]
        public void DisplayValueTests()
        {
            Assert.Equal(TestEnumConstants.CamelCasePropDisplay, TestEnumHelper.Consts.Display.CamelCaseProp);
            Assert.Equal(TestEnumConstants.UpperCasePropDisplay, TestEnumHelper.Consts.Display.UpperCaseProp);
        }

        [Fact]
        public void ValueValueTests()
        {
            Assert.Equal(TestEnumConstants.CamelCasePropValue, TestEnumHelper.Consts.Value.CamelCaseProp);
            Assert.Equal(TestEnumConstants.UpperCasePropValue, TestEnumHelper.Consts.Value.UpperCaseProp);
        }
    }
}
