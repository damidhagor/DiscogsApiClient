using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs003Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenMethodHasNoCancellationToken()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs003TestsSources.WhenMethodHasNoCancellationToken);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs003 = diagnostics.Where(d => d.Id == "DISCOGS003").ToArray();

        await Assert.That(discogs003.Length).IsEqualTo(1);
        await Assert.That(discogs003[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs003[0].GetMessage()).IsEqualTo(
            "Method 'GetTestAsync' must have a CancellationToken parameter — every async API method requires one for cancellation support");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenMethodHasCancellationToken()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs003TestsSources.WhenMethodHasCancellationToken);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS003")).IsFalse();
    }
}
