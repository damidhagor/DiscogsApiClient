using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs011Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenHttpMethodIsNotPartialDefinition()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs011TestsSources.WhenHttpMethodIsNotPartialDefinition);

        var diagnostics = result.Results.Single().Diagnostics;
        var diagnostic = diagnostics.Where(d => d.Id == "DISCOGS011").ToArray();

        await Assert.That(diagnostic.Length).IsEqualTo(1);
        await Assert.That(diagnostic[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic[0].GetMessage()).IsEqualTo("Method 'GetTestAsync' is marked with an HTTP attribute but is not a 'partial' method definition — the generated implementation cannot be emitted");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenHttpMethodIsPartialDefinition()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs011TestsSources.WhenHttpMethodIsPartialDefinition);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS011")).IsFalse();
    }
}
