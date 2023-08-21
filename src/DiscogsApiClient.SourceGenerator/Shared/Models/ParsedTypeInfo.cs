namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal class ParsedTypeInfo
{
    public string FullTypeName { get; private set; }

    public string Name { get; private set; }

    public string Namespace { get; private set; }

    public bool NeedsGlobalPrefix { get; private set; }

    public bool IsNullable { get; private set; }

    public List<ParsedTypeInfo> GenericTypeArguments { get; private set; }

    public bool IsGeneric { get; private set; }

    public List<EnumerationMember> EnumMembers { get; private set; }

    public bool IsEnum { get; private set; }

    public ParsedTypeInfo(
        string name,
        string @namespace,
        bool needsGlobalPrefix,
        bool isNullable,
        List<ParsedTypeInfo>? genericTypeArguments = null,
        List<EnumerationMember>? enumMembers = null)
    {
        Name = name;
        Namespace = @namespace;
        NeedsGlobalPrefix = needsGlobalPrefix;
        IsNullable = isNullable;
        GenericTypeArguments = genericTypeArguments ?? new();
        IsGeneric = GenericTypeArguments.Count > 0;
        EnumMembers = enumMembers ?? new();
        IsEnum = EnumMembers.Count > 0;
        FullTypeName = GetFullTypeName(true);
    }

    public string GetFullTypeName(bool includeNullable = true)
        => $"{(NeedsGlobalPrefix ? "global::" : "")}{Namespace}.{Name}{(IsGeneric ? $"<{string.Join(", ", GenericTypeArguments.Select(a => a.FullTypeName))}>" : "")}{(IsNullable && includeNullable ? "?" : "")}";

    public bool IsType<T>(bool genericComparison = false)
    {
        return IsType(typeof(T), genericComparison);
    }

    public bool IsType(Type type, bool genericComparison = false)
    {
        if (genericComparison)
        {
            if (type.GenericTypeArguments.Length != GenericTypeArguments.Count)
            {
                return false;
            }

            if (type.IsGenericType)
            {
                foreach (var genericTypeArg in type.GenericTypeArguments)
                {
                    if (!GenericTypeArguments.Any(a => a.IsType(genericTypeArg)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return IsType(type.Namespace, type.Name);
            }
        }
        else
        {
            return IsType(type.Namespace, type.Name);
        }
    }

    public bool IsType(string @namespace, string name)
    {
        return @namespace == Namespace && name == Name;
    }
}
