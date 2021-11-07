using Microsoft.CodeAnalysis;
using StringEnumGenerator.Generator;

namespace StringEnumGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            new GenerationOrchestrator(context).Generate();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}
