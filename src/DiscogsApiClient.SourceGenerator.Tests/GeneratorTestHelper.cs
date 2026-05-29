using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Tests;

internal static class GeneratorTestHelper
{
    private static readonly MetadataReference[] _compilationReferences =
    [
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),        // System.Runtime
        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),    // System.Linq
        MetadataReference.CreateFromFile(typeof(JsonSerializer).Assembly.Location) // System.Text.Json
    ];

    public static CSharpCompilation CreateCompilation(params ReadOnlySpan<string> sources)
    {
        var syntaxTrees = new SyntaxTree[sources.Length];
        for (var i = 0; i < sources.Length; i++)
        {
            syntaxTrees[i] = CSharpSyntaxTree.ParseText(sources[i]);
        }

        return CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: syntaxTrees,
            references: _compilationReferences,
            options: new(OutputKind.DynamicallyLinkedLibrary));
    }

    public static GeneratorDriverRunResult RunGenerator<TGenerator>(params ReadOnlySpan<string> sources)
        where TGenerator : IIncrementalGenerator, new()
    {
        var compilation = CreateCompilation(sources);
        var generator = new TGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: false));

        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out _);

        return driver.GetRunResult();
    }

    public static (GeneratorDriverRunResult FirstRunResult, GeneratorDriverRunResult SecondRunResult) RunGeneratorTwice<TGenerator>(
        string[] initialSources,
        string[] modifiedSources)
        where TGenerator : IIncrementalGenerator, new()
    {
        var compilation1 = CreateCompilation(initialSources);
        var generator = new TGenerator();

        var driver = CSharpGeneratorDriver.Create(
            generators: [generator.AsSourceGenerator()],
            driverOptions: new(trackIncrementalGeneratorSteps: true));

        var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation1, out _, out _);
        var firstRunResult = updatedDriver.GetRunResult();

        var compilation2 = CreateCompilation(modifiedSources);
        updatedDriver = updatedDriver.RunGeneratorsAndUpdateCompilation(compilation2, out _, out _);

        var secondRunResult = updatedDriver.GetRunResult();

        return (firstRunResult, secondRunResult);
    }

    public static string NormalizeSource(string source)
    {
        var lines = source
            .ReplaceLineEndings("\n")
            .Split("\n")
            .Select(l => l.Trim());

        return string.Join("\n", lines).Trim();
    }
}
