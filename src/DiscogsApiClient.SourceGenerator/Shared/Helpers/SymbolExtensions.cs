namespace DiscogsApiClient.SourceGenerator.Shared.Helpers;

internal static class SymbolExtensions
{
    public static bool TryGetConstFieldValue<T>(this INamedTypeSymbol? symbol, string fieldName, out T? value)
    {
        value = default;

        var inspectedSymbol = symbol;
        while (inspectedSymbol is not null)
        {
            var field = inspectedSymbol.GetMembers()
                .OfType<IFieldSymbol>()
                .FirstOrDefault(f => f.IsConst && f.Name == fieldName);

            if (field is not null)
            {
                if (field.ConstantValue is T castValue)
                {
                    value = castValue;
                    return true;
                }

                return false;
            }

            inspectedSymbol = inspectedSymbol.BaseType;
        }

        return false;
    }

    public static bool TryGetGenericTypeArgument(this INamedTypeSymbol? symbol, int index, out ITypeSymbol? genericArgument)
    {
        genericArgument = default;
        if (symbol is null
            || !symbol.IsGenericType
            || index >= symbol.Arity)
        {
            return false;
        }

        genericArgument = symbol.TypeArguments[index];
        return true;
    }
}
