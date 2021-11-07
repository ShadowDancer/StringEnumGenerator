using StringEnumGenerator.Tests.DiagnosticsTests.Tools;
using Xunit;

namespace StringEnumGenerator.Tests.DiagnosticsTests
{
    public class DuplicateMapping
    {
        [Fact]
        public void Should_ReportError_When_StringEnumIsNested()
        {
            string code = @"
namespace MyCode
{
    using StringEnumGenerator.Attributes;
    public enum DuplicatedProperties
    {
        [StringEnum(Display = ""Baz"")]
        Foo,
        [StringEnum(Display = ""Baz"")]
            Bar
    }
}
";


            var expectedDiagnostic = Diagnostics.DuplicatedEnumStringValue(null, "DuplicatedProperties", "Baz", "Display");
            var expectedDiagnostic2 = Diagnostics.DuplicatedEnumStringValue(null, "DuplicatedProperties", "Baz", "Display");

            SourceGeneratorRunner.RunCodeGeneratorForSingleResult(code, out _, out var diagnostics);

            Assert.Equal(2, diagnostics.Length);
            Assert.Equal(expectedDiagnostic, diagnostics[0]);
            Assert.Equal(expectedDiagnostic2, diagnostics[1]);
        }
    }
}
