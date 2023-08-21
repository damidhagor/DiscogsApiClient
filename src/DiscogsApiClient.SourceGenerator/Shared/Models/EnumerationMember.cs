namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal sealed class EnumerationMember
{
    public string MemberName { get; private set; }

    public string MemberNameAlias { get; private set; }

    public EnumerationMember(string memberName, string memberNameAlias)
    {
        MemberName = memberName;
        MemberNameAlias = memberNameAlias;
    }
}
