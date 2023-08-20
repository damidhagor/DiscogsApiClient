using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

internal sealed class Enumeration
{
    public ParsedTypeInfo TypeInfo { get; private set; }

    public List<EnumerationMember> Members { get; private set; }

    public Enumeration(ParsedTypeInfo typeInfo, List<EnumerationMember> members)
    {
        TypeInfo = typeInfo;
        Members = members;
    }
}
