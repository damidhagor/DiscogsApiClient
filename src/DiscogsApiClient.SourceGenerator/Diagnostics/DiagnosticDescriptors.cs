namespace DiscogsApiClient.SourceGenerator.Diagnostics;

internal static class DiagnosticDescriptors
{
    private const string Category = "DiscogsApiClient.SourceGenerator";

    public static readonly DiagnosticDescriptor InvalidMethodReturnType = new(
        "DISCOGS002",
        "API method must return Task or Task<T>",
        "Method '{0}' must return Task or Task<T> to be generated as an API method",
        Category,
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor RouteParameterMismatch = new(
        "DISCOGS004",
        "Route parameter not found in method parameters",
        "Route parameter '{0}' in '{1}' was not found in the method's parameters",
        Category,
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor EmptyEnum = new(
        "DISCOGS005",
        "Enum has no members",
        "Enum '{0}' has no members — the generated JSON converter will not handle any values",
        Category,
        DiagnosticSeverity.Warning,
        true);

    public static readonly DiagnosticDescriptor UnknownHttpMethod = new(
        "DISCOGS006",
        "Unknown HTTP method",
        "Method '{0}' has an unrecognized HTTP method type '{1}'",
        Category,
        DiagnosticSeverity.Warning,
        true);

    public static readonly DiagnosticDescriptor DuplicateBodyParameter = new(
        "DISCOGS007",
        "Duplicate [Body] parameter",
        "Method '{0}' has multiple [Body] parameters — only one is allowed",
        Category,
        DiagnosticSeverity.Error,
        true);

    public static readonly DiagnosticDescriptor MissingCancellationToken = new(
        "DISCOGS003",
        "API method has no CancellationToken parameter",
        "Method '{0}' has no CancellationToken parameter — consider adding one for proper cancellation support",
        Category,
        DiagnosticSeverity.Warning,
        true);

    public static readonly DiagnosticDescriptor UnsupportedQueryParameterType = new(
        "DISCOGS001",
        "Unsupported query parameter property type",
        "Property '{0}' on query parameter type '{1}' has unsupported type '{2}' and will be skipped — only string, int, and enum properties are supported",
        Category,
        DiagnosticSeverity.Warning,
        true);
}
