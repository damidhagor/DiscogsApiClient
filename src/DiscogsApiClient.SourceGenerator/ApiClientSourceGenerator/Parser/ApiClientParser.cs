﻿using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.Shared.Helpers;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Parser;

internal static class ApiClientParser
{
    public static ApiClient? ParseApiClient(this InterfaceDeclarationSyntax interfaceSyntax, Compilation compilation, CancellationToken cancellationToken)
    {
        var interfaceModel = compilation.GetSemanticModel(interfaceSyntax.SyntaxTree);
        if (interfaceModel.GetDeclaredSymbol(interfaceSyntax) is not INamedTypeSymbol interfaceSymbol)
        {
            return null;
        }

        interfaceSymbol.TryGetAttributeNamedArgument<string>(
            Constants.ApiClientNamespace,
            ApiCLientAttribute.Name,
            ApiCLientAttribute.NamePropertyName,
            out var clientName);
        interfaceSymbol.TryGetAttributeNamedArgument<string>(
            Constants.ApiClientNamespace,
            ApiCLientAttribute.Name,
            ApiCLientAttribute.NamespacePropertyName,
            out var clientNamespace);
        interfaceSymbol.TryGetAttributeConstructorArgument<INamedTypeSymbol>(
            Constants.ApiClientNamespace,
            ApiCLientAttribute.Name,
            out var jsonSerializerContextTypeSymbol);

        if (jsonSerializerContextTypeSymbol is null)
        {
            return null;
        }

        var typeInfo = interfaceSymbol.GetSymbolTypeInfo();
        var jsonSerializerContextTypeTypeInfo = jsonSerializerContextTypeSymbol.GetSymbolTypeInfo();

        clientName ??= typeInfo.Name.AsSpan().Slice(1, typeInfo.Name.Length - 1).ToString();
        clientNamespace ??= typeInfo.Namespace;

        var apiMethodsToGenerate = interfaceSymbol.ParseApiMethods(cancellationToken);

        return new(typeInfo, jsonSerializerContextTypeTypeInfo, clientName, clientNamespace, apiMethodsToGenerate);
    }
}
