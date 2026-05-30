using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs002Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenMethodReturnsVoid()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs002TestsSources.WhenMethodReturnsVoid);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs002 = diagnostics.Where(d => d.Id == "DISCOGS002").ToArray();

        await Assert.That(discogs002.Length).IsEqualTo(1);
        await Assert.That(discogs002[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs002[0].GetMessage()).IsEqualTo(
            "Method 'GetSync' must return Task or Task<T> to be generated as an API method");
    }

    [Test]
    public async Task ShouldReportDiagnostic_WhenMethodReturnsNonTaskType()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs002TestsSources.WhenMethodReturnsNonTaskType);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs002 = diagnostics.Where(d => d.Id == "DISCOGS002").ToArray();

        await Assert.That(discogs002.Length).IsEqualTo(1);
        await Assert.That(discogs002[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs002[0].GetMessage()).IsEqualTo(
            "Method 'GetSync' must return Task or Task<T> to be generated as an API method");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenMethodReturnsTask()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs002TestsSources.WhenMethodReturnsTask);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS002")).IsFalse();
    }
}
