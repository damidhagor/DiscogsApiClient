namespace DiscogsApiClient.SourceGenerator.Diagnostics;

internal sealed record DiagnosticLocation(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan)
{
    public static DiagnosticLocation From(Location location)
    {
        var lineSpan = location.GetLineSpan();
        return new(lineSpan.Path, location.SourceSpan, lineSpan.Span);
    }

    public Location ToLocation() => Location.Create(FilePath, TextSpan, LineSpan);
}
