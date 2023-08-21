namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal class ParsedParameterTypeInfo : ParsedTypeInfo
{
    public string ParameterName { get; private set; }

    public string ParameterNameAlias { get; private set; }

    public ParsedParameterTypeInfo(
        string parameterName,
        string parameterNameAlias,
        string name,
        string @namespace,
        bool needsGlobalPrefix,
        bool isNullable,
        List<ParsedTypeInfo>? genericTypeArguments = null,
        List<EnumerationMember>? enumMembers = null)
        : base(name, @namespace, needsGlobalPrefix, isNullable, genericTypeArguments, enumMembers)
    {
        ParameterName = parameterName;
        ParameterNameAlias = parameterNameAlias;
    }
}
