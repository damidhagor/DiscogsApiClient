using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class ApiClientPostInitializationTests
{
    [Test]
    public async Task ShouldGenerateApiClientAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == ApiClientAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(ApiClientAttribute.Source));
    }

    [Test]
    public async Task ShouldGenerateHttpMethodBaseAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == HttpMethodBaseAttribute.SourceHint)
            .SourceText.ToString();

        var expected = HttpMethodBaseAttribute.Source;
        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateHttpGetAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == HttpGetAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(HttpGetAttribute.Source));
    }

    [Test]
    public async Task ShouldGenerateHttpPostAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == HttpPostAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(HttpPostAttribute.Source));
    }

    [Test]
    public async Task ShouldGenerateHttpPutAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == HttpPutAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(HttpPutAttribute.Source));
    }

    [Test]
    public async Task ShouldGenerateHttpDeleteAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == HttpDeleteAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(HttpDeleteAttribute.Source));
    }

    [Test]
    public async Task ShouldGenerateBodyAttribute_WhenPostInit()
    {
        var result = GeneratorTestHelper.RunGenerator<ApiClientSourceGenerator.ApiClientSourceGenerator>("");
        var generated = result.Results.Single().GeneratedSources
            .First(s => s.HintName == BodyAttribute.SourceHint)
            .SourceText.ToString();

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(generated))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(BodyAttribute.Source));
    }

}
