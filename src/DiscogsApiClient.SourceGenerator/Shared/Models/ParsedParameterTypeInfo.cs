namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal sealed record ParsedParameterTypeInfo(
    string ParameterName,
    string ParameterNameAlias,
    ParsedTypeInfo TypeInfo)
{
    public string Name => TypeInfo.Name;

    public string Namespace => TypeInfo.Namespace;

    public string FullTypeName => TypeInfo.FullTypeName;

    public bool IsNullable => TypeInfo.IsNullable;

    public bool IsGeneric => TypeInfo.IsGeneric;

    public bool IsEnum => TypeInfo.IsEnum;

    public bool IsVoid => TypeInfo.IsVoid;

    public EquatableArray<ParsedTypeInfo> GenericTypeArguments => TypeInfo.GenericTypeArguments;

    public EquatableArray<EnumerationMember> EnumMembers => TypeInfo.EnumMembers;

    public string GetFullTypeName(bool includeNullable = true) => TypeInfo.GetFullTypeName(includeNullable);

    public bool IsType<T>() => TypeInfo.IsType<T>();

    public bool IsType(string @namespace, string name) => TypeInfo.IsType(@namespace, name);
}
