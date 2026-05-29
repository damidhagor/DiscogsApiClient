namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs004Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenRouteParamHasNoMatchingMethodParam()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs004Tests_WhenRouteParamHasNoMatchingMethodParam);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs004 = diagnostics.Where(d => d.Id == "DISCOGS004").ToArray();

        await Assert.That(discogs004.Length).IsEqualTo(1);
        await Assert.That(discogs004[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs004[0].GetMessage())
            .IsEqualTo("Route parameter 'id' in '/items/{id}' was not found in the method's parameters");
    }

    [Test]
    public async Task ShouldReportDiagnostic_ForEachMissingRouteParam()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs004Tests_WhenMultipleRouteParamsOneMissing);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs004 = diagnostics.Where(d => d.Id == "DISCOGS004").ToArray();

        await Assert.That(discogs004.Length).IsEqualTo(1);
        await Assert.That(discogs004[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs004[0].GetMessage())
            .IsEqualTo("Route parameter 'itemId' in '/users/{userId}/items/{itemId}' was not found in the method's parameters");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenAllRouteParamsHaveMatchingMethodParams()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs004Tests_WhenAllRouteParamsHaveMatchingMethodParams);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS004")).IsFalse();
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenRouteHasNoParams()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs004Tests_WhenRouteHasNoParams);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS004")).IsFalse();
    }
}
