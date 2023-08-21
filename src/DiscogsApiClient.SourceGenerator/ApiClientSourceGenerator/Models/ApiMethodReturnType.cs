using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;

internal sealed class ApiMethodReturnType
{
    public ParsedTypeInfo TypeInfo { get; private set; }

    public bool IsVoid { get; private set; }

    public bool IsTask { get; private set; }

    public bool IsTaskWithResult { get; private set; }

    public ApiMethodReturnType(ParsedTypeInfo typeInfo, bool isVoid)
    {
        TypeInfo = typeInfo;
        IsVoid = isVoid;
        IsTask = typeInfo.IsType<Task>();
        IsTaskWithResult = IsTask && typeInfo.GenericTypeArguments.Count == 1;
    }
}
