namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs007Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenMethodHasMultipleBodyParams()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs007Tests_WhenMethodHasMultipleBodyParams);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs007 = diagnostics.Where(d => d.Id == "DISCOGS007").ToArray();

        await Assert.That(discogs007.Length).IsEqualTo(1);
        await Assert.That(discogs007[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(discogs007[0].GetMessage())
            .IsEqualTo("Method 'CreateAsync' has multiple [Body] parameters — only one is allowed");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenMethodHasSingleBodyParam()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs007Tests_WhenMethodHasSingleBodyParam);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS007")).IsFalse();
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenMethodHasNoBodyParam()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs007Tests_WhenMethodHasNoBodyParam);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS007")).IsFalse();
    }
}
