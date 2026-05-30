using DiscogsApiClient.SourceGenerator.Shared;
using DiscogsApiClient.SourceGenerator.Shared.Attributes;
using DiscogsApiClient.SourceGenerator.Tests.Shared.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.Shared;

public sealed class SharedSourceGeneratorTests
{
    [Test]
    public async Task ShouldGenerateAliasAsAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<SharedSourceGenerator>(
            SharedSourceGeneratorTestsSources.EmptySource);

        await Assert.That(result.Diagnostics).IsEmpty();

        var source = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "AliasAsAttribute.g.cs")
            .SourceText.ToString();

        var expected = AliasAsAttribute.Source;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }
}
