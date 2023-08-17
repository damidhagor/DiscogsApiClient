using System.Collections.Immutable;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Parser;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

[Generator]
public class JsonSerializationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);

        var enumDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, cancellationToken) => IsSyntaxNodeGenerationTarget(syntaxNode),
            static (syntaxContext, cancellationToken) => GetSemanticTargetForGeneration(syntaxContext))
            .Where(static syntax => syntax is not null);

        var compilationAndEnums = context.CompilationProvider.Combine(enumDeclarations.Collect());

        context.RegisterSourceOutput(
            compilationAndEnums,
            static (sourceProductionContext, source) => Execute(source.Left, source.Right!, sourceProductionContext));
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddGenerateJsonConverterAttribute();
    }

    private static bool IsSyntaxNodeGenerationTarget(SyntaxNode syntaxNode)
    {
        return syntaxNode is EnumDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static EnumDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var interfaceSyntax = (EnumDeclarationSyntax)context.Node;

        foreach (var attributeListSyntax in interfaceSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (IsMarkerAttribute(attributeSyntax, context))
                {
                    return interfaceSyntax;
                }
            }
        }

        return null;
    }

    private static bool IsMarkerAttribute(AttributeSyntax attributeSyntax, GeneratorSyntaxContext context)
    {
        return context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is IMethodSymbol attributeSymbol
                    && attributeSymbol.ContainingNamespace.ToDisplayString() == Constants.JsonSerializationNamespace
                    && attributeSymbol.ContainingType.Name == GenerateJsonConverterAttribute.Name;
    }

    private static void Execute(Compilation compilation, ImmutableArray<EnumDeclarationSyntax> enumDeclarations, SourceProductionContext context)
    {
        if (enumDeclarations.IsDefaultOrEmpty)
        {
            return;
        }

        var enums = new List<Enumeration>();
        foreach (var enumDeclaration in enumDeclarations.Distinct())
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var enumeration = enumDeclaration.ParseEnum(compilation, context.CancellationToken);
            if (enumeration is not null)
            {
                enums.Add(enumeration);
            }
        }

        var (hint, source) = enums.GenerateJsonConverters(context.CancellationToken);
        context.AddSource(hint, source);

        using var writer = FileOutputDebugHelper.GetOutputStreamWriter($"converter.txt", false);
        writer.WriteLine(source.ToString());
    }
}
