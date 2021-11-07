using System.Linq;
using Xunit;
using StringEnumGenerator.Tests.DiagnosticsTests.Tools;

namespace StringEnumGenerator.Tests.DiagnosticsTests
{
    public class EnumInPartialClass
    {
        [Fact]
        public void Should_ReportError_When_StringEnumIsNested()
        {
            string code = @"
namespace MyCode
{
    using StringEnumGenerator.Attributes;
    public class Foo
    {
        [System.Flags]
        public enum NestedEnum
        {
            [StringEnum(Display = ""d"", Value = ""v"")]
            Carrot = 1,

        }
    }
}
";
            var expectedDiagnostic = Diagnostics.EnumInPartialClass(null, "NestedEnum");

            _ = SourceGeneratorRunner.RunCodeGeneratorForSingleResult(code, out _, out var diagnostics);

            diagnostics.Single();

            Assert.Equal(expectedDiagnostic, diagnostics[0]);


        }
    }
}
