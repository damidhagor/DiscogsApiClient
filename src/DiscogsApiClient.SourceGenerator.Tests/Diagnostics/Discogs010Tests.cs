using DiscogsApiClient.SourceGenerator.Tests.Diagnostics.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs010Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenApiClientHasNoContextMember()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs010TestsSources.WhenApiClientHasNoContextMember);

        var diagnostics = result.Results.Single().Diagnostics;
        var diagnostic = diagnostics.Where(d => d.Id == "DISCOGS010").ToArray();

        await Assert.That(diagnostic.Length).IsEqualTo(1);
        await Assert.That(diagnostic[0].Severity).IsEqualTo(DiagnosticSeverity.Error);
        await Assert.That(diagnostic[0].GetMessage()).IsEqualTo("Class 'TestApiClient' must declare a field or property of type 'global::TestNamespace.TestJsonContext' (or a type deriving from it) for the generated code to resolve JSON type metadata");
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenApiClientHasContextMember()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>(
            Discogs010TestsSources.WhenApiClientHasContextMember);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS010")).IsFalse();
    }
}