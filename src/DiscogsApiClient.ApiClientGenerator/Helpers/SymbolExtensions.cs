using System.Diagnostics.CodeAnalysis;

namespace DiscogsApiClient.ApiClientGenerator.Helpers;

internal static class SymbolExtensions
{
    public static bool TryGetAttribute(
        this ISymbol symbol,
        string attributeNamespace,
        string attributeName,
        out AttributeData? attributeData)
    {
        attributeData = symbol
            .GetAttributes()
            .FirstOrDefault(b => b.AttributeClass?.Name == attributeName
                              && b.AttributeClass?.ContainingNamespace.ToDisplayString() == attributeNamespace);

        return attributeData is not null;
    }

    public static bool TryGetAttributeBase(
        this ISymbol symbol,
        string attributeNamespace,
        string attributeName,
        out AttributeData? attributeData)
    {
        attributeData = symbol
            .GetAttributes()
            .FirstOrDefault(b => b.AttributeClass?.BaseType?.Name == attributeName
                              && b.AttributeClass?.BaseType?.ContainingNamespace.ToDisplayString() == attributeNamespace);

        return attributeData is not null;
    }


    public static bool TryGetAttributeConstructorArgument<T>(this AttributeData attributeData, out T? value)
        => TryGetAttributeConstructorArgument(attributeData, 0, out value);

    public static bool TryGetAttributeConstructorArgument<T>(this AttributeData attributeData, int index, out T? value)
    {
        if (attributeData.ConstructorArguments.Length <= index)
        {
            value = default;
            return false;
        }

        var argument = attributeData.ConstructorArguments[index].Value;

        if (argument is T castValue)
        {
            value = castValue;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }
}
