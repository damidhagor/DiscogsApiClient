namespace DiscogsApiClient.SourceGenerator.Shared.Models;

internal sealed class ParsedTypeInfo
{
    public string Name { get; private set; }

    public string Namespace { get; private set; }

    public bool NeedsGlobalPrefix { get; private set; }

    public string GetFullTypeName() => $"{(NeedsGlobalPrefix ? "global::" : "")}{Namespace}.{Name}";

    public ParsedTypeInfo(string name, string @namespace, bool needsGlobalPrefix)
    {
        Name = name;
        Namespace = @namespace;
        NeedsGlobalPrefix = needsGlobalPrefix;
    }
}
