namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

internal sealed class Enumeration
{
    public string FullName { get; private set; }

    public string NamespaceName { get; private set; }

    public string EnumName { get; private set; }

    public List<EnumerationMember> Members { get; private set; }

    public Enumeration(string fullName, string namespaceName, string enumName, List<EnumerationMember> members)
    {
        FullName = fullName;
        NamespaceName = namespaceName;
        EnumName = enumName;
        Members = members;
    }
}
