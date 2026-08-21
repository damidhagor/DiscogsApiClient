namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal sealed record ParsedTypeInfo(
    string Name,
    string Namespace,
    bool NeedsGlobalPrefix,
    bool IsNullable,
    EquatableArray<ParsedTypeInfo> GenericTypeArguments,
    EquatableArray<EnumerationMember> EnumMembers)
{
    public string FullTypeName { get; }
        = GetFullTypeName(Name, Namespace, NeedsGlobalPrefix, IsNullable, GenericTypeArguments, true);

    public bool IsGeneric { get; } = GenericTypeArguments.Length > 0;

    public bool IsEnum { get; } = EnumMembers.Length > 0;

    public bool IsVoid { get; } = IsType(Namespace, Name, typeof(void));

    public string GetFullTypeName(bool includeNullable = true)
        => GetFullTypeName(Name, Namespace, NeedsGlobalPrefix, IsNullable, GenericTypeArguments, includeNullable);

    private static string GetFullTypeName(
        string name,
        string @namespace,
        bool needsGlobalPrefix,
        bool isNullable,
        EquatableArray<ParsedTypeInfo> genericTypeArguments,
        bool includeNullable)
    {
        var prefix = needsGlobalPrefix ? "global::" : "";
        var genericPart = genericTypeArguments.Length > 0
            ? $"<{string.Join(", ", genericTypeArguments.Select(a => a.FullTypeName))}>"
            : "";
        var nullableSuffix = isNullable && includeNullable ? "?" : "";

        return $"{prefix}{@namespace}.{name}{genericPart}{nullableSuffix}";
    }

    public bool IsType<T>() => IsType(typeof(T).Namespace, typeof(T).Name);

    public bool IsType(string @namespace, string name) => @namespace == Namespace && name == Name;

    private static bool IsType(string @namespace, string name, Type type)
        => type.Namespace == @namespace && type.Name == name;
}
