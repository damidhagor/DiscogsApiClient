using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp;

namespace DiscogsApiClient.SourceGenerator.Tests;

internal static class GeneratorTestHelper
{
    private static readonly MetadataReference[] CompilationReferences =
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
            references: CompilationReferences,
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

    public static string NormalizeLineEndings(string source)
        => source.ReplaceLineEndings("\n");
}
