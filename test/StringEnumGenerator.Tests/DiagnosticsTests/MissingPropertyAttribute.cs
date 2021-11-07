using System.Linq;
using Xunit;
using StringEnumGenerator.Tests.DiagnosticsTests.Tools;

namespace StringEnumGenerator.Tests.DiagnosticsTests
{
    public class MissingPropertyAttribute
    {
        [Fact]
        public void Should_ReportError_When_StringEnumIsNested()
        {
            string code = @"
namespace MyCode
{
    using StringEnumGenerator.Attributes;

    public enum UnannotatedProperty
    {
        [StringEnum(Display = ""Foo"")]
        Bar,
        Baz
    }
}
";

            var expectedDiagnostic = Diagnostics.MissingPropertyAnnotation(null, "UnannotatedProperty", "Baz", "Display");

            SourceGeneratorRunner.RunCodeGeneratorForSingleResult(code, out _, out var diagnostics);

            diagnostics.Single();

            Assert.Equal(expectedDiagnostic, diagnostics[0]);
        }
    }
}
