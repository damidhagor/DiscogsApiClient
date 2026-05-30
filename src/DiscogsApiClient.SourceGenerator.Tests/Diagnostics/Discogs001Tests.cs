using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs001Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenQueryParamHasUnsupportedPropertyType()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs001TestsSources.WhenQueryParamHasUnsupportedPropertyType);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs001 = diagnostics.Where(d => d.Id == "DISCOGS001").ToArray();

        await Assert.That(discogs001.Length).IsEqualTo(1);
        await Assert.That(discogs001[0].Severity).IsEqualTo(DiagnosticSeverity.Warning);
        await Assert.That(discogs001[0].GetMessage()).IsEqualTo(
            "Property 'IsActive' on query parameter type 'QueryParams' has unsupported type 'bool' and will be skipped — only string, int, and enum properties are supported");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenAllQueryParamPropertiesAreSupported()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs001TestsSources.WhenAllQueryParamPropertiesAreSupported);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS001")).IsFalse();
    }
}
