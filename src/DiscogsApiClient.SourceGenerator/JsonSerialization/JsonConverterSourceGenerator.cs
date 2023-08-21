using System.Collections.Immutable;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Attributes;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Parser;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

[Generator]
public class JsonConverterSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(DoPostInitialization);

        var declarations = context.SyntaxProvider.CreateSyntaxProvider(
            static (syntaxNode, cancellationToken) => IsSyntaxNodeGenerationTarget(syntaxNode),
            static (syntaxContext, cancellationToken) => GetSemanticTargetForGeneration(syntaxContext))
            .Where(static syntax => syntax is not null);

        var compilationAndDeclarations = context.CompilationProvider.Combine(declarations.Collect());

        context.RegisterSourceOutput(
            compilationAndDeclarations,
            static (sourceProductionContext, source) => Execute(source.Left, source.Right!, sourceProductionContext));
    }

    private static void DoPostInitialization(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddGenerateJsonConverterAttribute();
    }

    private static bool IsSyntaxNodeGenerationTarget(SyntaxNode syntaxNode)
    {
        return syntaxNode is MemberDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    private static MemberDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var memberSyntax = (MemberDeclarationSyntax)context.Node;

        return memberSyntax.HasMarkerAttribute(context, Constants.JsonSerializationNamespace, GenerateJsonConverterAttribute.Name)
            ? memberSyntax
            : null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<MemberDeclarationSyntax> memberDeclarations, SourceProductionContext context)
    {
        var distinctMemberDeclarations = memberDeclarations.Distinct();

        HandleEnums(compilation, distinctMemberDeclarations, context);
    }

    private static void HandleEnums(Compilation compilation, IEnumerable<MemberDeclarationSyntax> memberDeclarations, SourceProductionContext context)
    {
        var enums = new List<Enumeration>();
        foreach (var enumDeclaration in memberDeclarations.OfType<EnumDeclarationSyntax>())
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var enumeration = enumDeclaration.ParseEnum(compilation, context.CancellationToken);
            if (enumeration is not null)
            {
                enums.Add(enumeration);
            }
        }

        var (hint, source) = enums.GenerateEnumJsonConverters(context.CancellationToken);
        context.AddSource(hint, source);
    }
}
