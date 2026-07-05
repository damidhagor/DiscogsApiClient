using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiMethodGenerator
{
    private const string _partial = "partial ";
    private const string _async = "async ";
    private const string _methodBodyStart =
        """
            {
        """;
    private const string _methodBodyEnd =
        """
            }
        """;
    private const string _openParenthesis = "(";
    private const string _closedParenthesis = ")";
    private const string _space = " ";
    private const string _indent = "    ";
    private const string _parameterSeparator = ", ";

    public static void GenerateApiMethod(this StringBuilder builder, ApiMethod apiMethod, ApiClient apiClient, CancellationToken cancellationToken)
    {
        builder.AppendLine();

        builder.Append(_indent);
        builder.AppendLine(Constants.GeneratedCodeAttribute);

        builder.Append(_indent);
        builder.Append(apiMethod.AccessModifier);
        builder.Append(_space);

        if (apiMethod.ReturnType.IsTask)
        {
            builder.Append(_async);
        }

        builder.Append(_partial);
        builder.GenerateMethodReturnType(apiMethod.ReturnType);
        builder.Append(apiMethod.Name);
        builder.GenerateMethodParameters(apiMethod.Parameters, cancellationToken);

        builder.AppendLine(_methodBodyStart);
        builder.GenerateApiMethodBody(apiMethod, apiClient);
        builder.AppendLine(_methodBodyEnd);
    }

    private static void GenerateMethodReturnType(this StringBuilder builder, ApiMethodReturnType returnType)
    {
        if (returnType.TypeInfo.IsVoid)
        {
            builder.Append("void");
        }
        else
        {
            builder.Append(returnType.TypeInfo.FullTypeName);
        }

        builder.Append(_space);
    }

    private static void GenerateMethodParameters(this StringBuilder builder, EquatableArray<ApiMethodParameter> parameters, CancellationToken cancellationToken)
    {
        builder.Append(_openParenthesis);

        for (var i = 0; i < parameters.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameter = parameters[i];

            builder.Append(parameter.TypeInfo.FullTypeName);
            builder.Append(_space);
            builder.Append(parameter.TypeInfo.ParameterName);

            if (i < parameters.Length - 1)
            {
                builder = builder.Append(_parameterSeparator);
            }
        }

        builder.AppendLine(_closedParenthesis);
    }
}
