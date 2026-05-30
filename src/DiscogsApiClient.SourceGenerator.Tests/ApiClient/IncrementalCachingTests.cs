using DiscogsApiClient.SourceGenerator.Tests.ApiClient.Sources;
using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class IncrementalCachingTests
{
    [Test]
    public async Task ShouldCacheTransformStep_WhenSourceUnchanged()
    {
        var generator = new ApiClientSourceGenerator.ApiClientSourceGenerator();
        var compilation1 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceUnchanged_First]);
        var compilation2 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceUnchanged_Second]);

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var runResult = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _)
                              .RunGeneratorsAndUpdateCompilation(compilation2, out _, out _)
                              .GetRunResult();

        var trackedSteps = runResult.Results.Single().TrackedSteps;

        await Assert.That(trackedSteps).ContainsKey("ApiClientTransform");

        await Assert.That(trackedSteps["ApiClientTransform"])
                    .All(s => s.Outputs.All(o => o.Reason is IncrementalStepRunReason.Cached
                                                          or IncrementalStepRunReason.Unchanged));
    }

    [Test]
    public async Task ShouldNotCacheTransformStep_WhenSourceChanged()
    {
        var generator = new ApiClientSourceGenerator.ApiClientSourceGenerator();
        var compilation1 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceChanged_First]);
        var compilation2 = GeneratorTestHelper.CreateCompilation([IncrementalCachingTestsSources.WhenSourceChanged_Second]);

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var runResult = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _)
                              .RunGeneratorsAndUpdateCompilation(compilation2, out _, out _)
                              .GetRunResult();

        var trackedSteps = runResult.Results.Single().TrackedSteps;

        await Assert.That(trackedSteps).ContainsKey("ApiClientTransform");

        await Assert.That(trackedSteps["ApiClientTransform"])
                    .All(s => s.Outputs.All(o => o.Reason is not IncrementalStepRunReason.Cached));
    }
}
