﻿using DiscogsApiClient.ApiClientGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiscogsApiClient.ApiClientGenerator.Parser;

internal static class ApiParser
{
    public static ApiClient? ParseApiClient(this InterfaceDeclarationSyntax interfaceSyntax, Compilation compilation, CancellationToken cancellationToken)
    {
        var interfaceModel = compilation.GetSemanticModel(interfaceSyntax.SyntaxTree);
        if (interfaceModel.GetDeclaredSymbol(interfaceSyntax) is not INamedTypeSymbol interfaceSymbol)
        {
            return null;
        }

        var interfaceName = interfaceSymbol.Name;
        var interfaceNamespace = interfaceSymbol.ContainingNamespace.ToDisplayString();
        var implementationName = interfaceName.AsSpan().Slice(1, interfaceName.Length - 1).ToString();
        var apiMethodsToGenerate = interfaceSymbol.ParseApiMethods(cancellationToken);

        return new(interfaceNamespace, implementationName, interfaceName, apiMethodsToGenerate);
    }
}
