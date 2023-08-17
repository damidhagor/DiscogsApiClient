using DiscogsApiClient.SourceGenerator.Shared.Generators;

namespace DiscogsApiClient.SourceGenerator.Shared;

[Generator]
public class SharedSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddAliasAsAttribute();
    }
}
