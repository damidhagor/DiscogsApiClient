namespace DiscogsApiClient.SourceGenerator.Diagnostics;

internal sealed record DiagnosticInfo(
    DiagnosticDescriptor Descriptor,
    DiagnosticLocation Location,
    EquatableArray<string> MessageArgs = default)
{
    public Diagnostic ToDiagnostic() => Diagnostic.Create(Descriptor, Location.ToLocation(), [.. MessageArgs]);
}
