using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

internal sealed class Enumeration
{
    public ParsedTypeInfo TypeInfo { get; private set; }

    public Enumeration(ParsedTypeInfo typeInfo)
    {
        TypeInfo = typeInfo;
    }
}
