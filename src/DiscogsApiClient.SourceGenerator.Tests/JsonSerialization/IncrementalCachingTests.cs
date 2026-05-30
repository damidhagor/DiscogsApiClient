using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Tests.JsonSerialization.Sources;
using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Tests.JsonSerialization;

public sealed class IncrementalCachingTests
{
    [Test]
    public async Task ShouldCacheTransformStep_WhenSourceUnchanged()
    {
        var sources = new[] { IncrementalCachingTestsSources.WhenMinimalEnum };

        var compilation1 = GeneratorTestHelper.CreateCompilation(sources);
        var generator = new JsonConverterSourceGenerator();

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _);

        var compilation2 = GeneratorTestHelper.CreateCompilation(sources);
        updatedDriver = updatedDriver.RunGeneratorsAndUpdateCompilation(compilation2, out _, out _);

        var secondRun = updatedDriver.GetRunResult();
        var trackedSteps = secondRun.Results.Single().TrackedSteps;

        if (trackedSteps.TryGetValue("EnumTransform", out var transformSteps))
        {
            foreach (var step in transformSteps)
            {
                foreach (var output in step.Outputs)
                {
                    await Assert.That(output.Reason)
                        .IsEqualTo(IncrementalStepRunReason.Cached)
                        .Or.IsEqualTo(IncrementalStepRunReason.Unchanged);
                }
            }
        }
    }

    [Test]
    public async Task ShouldNotCacheTransformStep_WhenSourceChanged()
    {
        var initialSource = IncrementalCachingTestsSources.WhenMinimalEnum;

        var modifiedSource =
            """
            using DiscogsApiClient.SourceGenerator.JsonSerialization;

            namespace TestNamespace;

            [GenerateJsonConverter]
            public enum TestStatus
            {
                Active,
                Inactive,
                Pending,
                Suspended
            }
            """;

        var compilation1 = GeneratorTestHelper.CreateCompilation([initialSource]);
        var generator = new JsonConverterSourceGenerator();

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _);

        var compilation2 = GeneratorTestHelper.CreateCompilation([modifiedSource]);
        updatedDriver = updatedDriver.RunGeneratorsAndUpdateCompilation(compilation2, out _, out _);

        var secondRun = updatedDriver.GetRunResult();
        var trackedSteps = secondRun.Results.Single().TrackedSteps;

        if (trackedSteps.TryGetValue("EnumTransform", out var transformSteps))
        {
            var hasNonCached = transformSteps
                .SelectMany(s => s.Outputs)
                .Any(o => o.Reason != IncrementalStepRunReason.Cached);

            await Assert.That(hasNonCached).IsTrue();
        }
    }
}
