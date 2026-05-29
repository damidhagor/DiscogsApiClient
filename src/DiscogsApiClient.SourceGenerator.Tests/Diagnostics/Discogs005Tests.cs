using DiscogsApiClient.SourceGenerator.JsonSerialization;

namespace DiscogsApiClient.SourceGenerator.Tests.Diagnostics;

public sealed class Discogs005Tests
{
    [Test]
    public async Task ShouldReportDiagnostic_WhenEnumHasNoMembers()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            DiagnosticsSources.Discogs005Tests_WhenEnumIsEmpty);

        var diagnostics = result.Results.Single().Diagnostics;
        var discogs005 = diagnostics.Where(d => d.Id == "DISCOGS005").ToArray();

        await Assert.That(discogs005.Length).IsEqualTo(1);
        await Assert.That(discogs005[0].Severity).IsEqualTo(DiagnosticSeverity.Warning);
        await Assert.That(discogs005[0].GetMessage())
            .IsEqualTo("Enum 'EmptyEnum' has no members — the generated JSON converter will not handle any values");
    }

    [Test]
    public async Task ShouldNotGenerateConverterOutput_WhenEnumIsEmpty()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            DiagnosticsSources.Discogs005Tests_WhenEnumIsEmpty);

        var converterOutputs = result.Results.Single().GeneratedSources
            .Where(s => s.HintName == "EnumJsonConverters.g.cs")
            .ToArray();

        await Assert.That(converterOutputs).IsEmpty();
    }

    [Test]
    public async Task ShouldNotReportDiagnostic_WhenEnumHasMembers()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            DiagnosticsSources.Discogs005Tests_WhenEnumHasMembers);

        var diagnostics = result.Results.Single().Diagnostics;

        await Assert.That(diagnostics.Any(d => d.Id == "DISCOGS005")).IsFalse();
    }
}
