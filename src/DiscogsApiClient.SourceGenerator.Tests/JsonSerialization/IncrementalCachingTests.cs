using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Tests.JsonSerialization.Sources;
using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Tests.JsonSerialization;

public sealed class IncrementalCachingTests
{
    [Test]
    public async Task ShouldCacheTransformStep_WhenSourceUnchanged()
    {
        var generator = new JsonConverterSourceGenerator();
        var compilation1 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceUnchanged_First]);
        var compilation2 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceUnchanged_Second]);

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var runResult = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _)
                              .RunGeneratorsAndUpdateCompilation(compilation2, out _, out _)
                              .GetRunResult();

        var trackedSteps = runResult.Results.Single().TrackedSteps;

        await Assert.That(trackedSteps).ContainsKey("EnumTransform");

        await Assert.That(trackedSteps["EnumTransform"])
                    .All(s => s.Outputs.All(o => o.Reason is IncrementalStepRunReason.Cached
                                                          or IncrementalStepRunReason.Unchanged));
    }

    [Test]
    public async Task ShouldNotCacheTransformStep_WhenSourceChanged()
    {
        var generator = new JsonConverterSourceGenerator();
        var compilation1 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceChanged_First]);
        var compilation2 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceChanged_Second]);

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var runResult = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _)
                              .RunGeneratorsAndUpdateCompilation(compilation2, out _, out _)
                              .GetRunResult();

        var trackedSteps = runResult.Results.Single().TrackedSteps;

        await Assert.That(trackedSteps).ContainsKey("EnumTransform");

        await Assert.That(trackedSteps["EnumTransform"])
                    .All(s => s.Outputs.All(o => o.Reason is not IncrementalStepRunReason.Cached));
    }
}
