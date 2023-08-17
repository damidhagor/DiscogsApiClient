namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

internal sealed class EnumerationMember
{
    public string FieldName { get; private set; }

    public string DisplayName { get; private set; }

    public EnumerationMember(string fieldName, string displayName)
    {
        FieldName = fieldName;
        DisplayName = displayName;
    }
}
