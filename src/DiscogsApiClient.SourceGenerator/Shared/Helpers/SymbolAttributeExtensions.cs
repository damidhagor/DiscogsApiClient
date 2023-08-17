namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class SymbolAttributeExtensions
{
    public static bool HasAttribute(
        this ISymbol? symbol,
        string attributeNamespace,
        string attributeName)
    {
        return symbol.TryGetAttribute(attributeNamespace, attributeName, out _);
    }

    public static bool TryGetAttribute(
        this ISymbol? symbol,
        string attributeNamespace,
        string attributeName,
        out AttributeData? attributeData)
    {
        attributeData = null;
        if (symbol == null)
        {
            return false;
        }

        foreach (var attribute in symbol.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            while (attributeClass is not null)
            {
                if (attributeClass.Name == attributeName
                    && attributeClass.ContainingNamespace.ToDisplayString() == attributeNamespace)
                {
                    attributeData = attribute;
                    return true;
                }

                attributeClass = attributeClass.BaseType;
            }
        }

        return false;
    }

    public static bool TryGetAttributeConstructorArgument<T>(
        this ISymbol? symbol,
        string attributeNamespace,
        string attributeName,
        out T? value)
        => symbol.TryGetAttributeConstructorArgument(attributeNamespace, attributeName, 0, out value);

    public static bool TryGetAttributeConstructorArgument<T>(
        this ISymbol? symbol,
        string attributeNamespace,
        string attributeName,
        int index,
        out T? value)
    {
        value = default;

        if (!symbol.TryGetAttribute(
            attributeNamespace,
            attributeName,
            out var attribute))
        {
            return false;
        }

        return attribute.TryGetAttributeConstructorArgument(index, out value);
    }

    public static bool TryGetAttributeConstructorArgument<T>(this AttributeData? attributeData, out T? value)
        => attributeData.TryGetAttributeConstructorArgument(0, out value);

    public static bool TryGetAttributeConstructorArgument<T>(this AttributeData? attributeData, int index, out T? value)
    {
        value = default;

        if (attributeData is null
            || attributeData.ConstructorArguments.Length <= index)
        {
            return false;
        }

        var argument = attributeData.ConstructorArguments[index].Value;

        if (argument is T castValue)
        {
            value = castValue;
            return true;
        }

        return false;
    }

    public static bool TryGetConstAttributeFieldValue<T>(
        this ISymbol? symbol,
        string attributeNamespace,
        string attributeName,
        string fieldName,
        out T? value)
    {
        value = default;

        if (!symbol.TryGetAttribute(
            attributeNamespace,
            attributeName,
            out var attribute))
        {
            return false;
        }

        return attribute!.AttributeClass.TryGetConstFieldValue(fieldName, out value);
    }
}
