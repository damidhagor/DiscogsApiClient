using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs008Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenApiClientClassIsNotPartial()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs008TestsSources.WhenApiClientClassIsNotPartial);

        var diagnostics = result.Results.Single().Diagnostics;
        var diagnostic = diagnostics.Where(d => d.Id == "DISCOGS008").ToArray();

        await Assert.That(diagnostic.Length).IsEqualTo(1);
        await Assert.That(diagnostic[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic[0].GetMessage()).IsEqualTo("Class 'TestApiClient' is marked with [ApiClient] but is not declared 'partial' — the generated methods cannot be emitted");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenApiClientClassIsPartial()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs008TestsSources.WhenApiClientClassIsPartial);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS008")).IsFalse();
    }
}
