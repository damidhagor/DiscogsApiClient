using DiscogsApiClient.SourceGenerator.Diagnostics;
using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Parser;

internal static class EnumParser
{
    public static GeneratorResult<Enumeration>? ParseEnum(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol enumSymbol)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var typeInfo = enumSymbol.GetSymbolTypeInfo();
        var location = DiagnosticLocation.From(context.TargetNode.GetLocation());

        return typeInfo.EnumMembers.Length == 0
            ? GeneratorResult<Enumeration>.Failure([new(DiagnosticDescriptors.EmptyEnum, location, [enumSymbol.Name])])
            : GeneratorResult<Enumeration>.Success(new(typeInfo));
    }
}
