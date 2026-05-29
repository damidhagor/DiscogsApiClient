namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs006Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenHttpMethodTypeIsUnrecognized()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs006Tests_WhenHttpMethodTypeIsUnrecognized);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs006 = diagnostics.Where(d => d.Id == "DISCOGS006").ToArray();

        await Assert.That(discogs006.Length).IsEqualTo(1);
        await Assert.That(discogs006[0].Severity).IsEqualTo(DiagnosticSeverity.Warning);
        await Assert.That(discogs006[0].GetMessage())
            .IsEqualTo("Method 'PatchTestAsync' has an unrecognized HTTP method type 'unrecognized'");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenHttpMethodsAreStandard()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            DiagnosticsSources.Discogs006Tests_WhenHttpMethodsAreStandard);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS006")).IsFalse();
    }
}
