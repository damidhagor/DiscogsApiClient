using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs009Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenApiClientHasNoHttpClientMember()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs009TestsSources.WhenApiClientHasNoHttpClientMember);

        var diagnostics = result.Results.Single().Diagnostics;
        var diagnostic = diagnostics.Where(d => d.Id == "DISCOGS009").ToArray();

        await Assert.That(diagnostic.Length).IsEqualTo(1);
        await Assert.That(diagnostic[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic[0].GetMessage()).IsEqualTo("Class 'TestApiClient' must declare a field or property of type 'System.Net.Http.HttpClient' for the generated code to send requests");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenApiClientHasHttpClientMember()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs009TestsSources.WhenApiClientHasHttpClientMember);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS009")).IsFalse();
    }
}