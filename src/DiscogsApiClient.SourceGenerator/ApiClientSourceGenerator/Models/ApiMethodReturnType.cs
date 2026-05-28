using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed record ApiMethodReturnType(ParsedTypeInfo TypeInfo)
{
    public bool IsTask { get; } = TypeInfo.IsType<Task>();
    
    public bool IsTaskWithResult { get; } = TypeInfo.IsType<Task>() && TypeInfo.GenericTypeArguments.Length == 1;
}
