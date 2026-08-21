using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiMethodGenerator
{
    private const string Partial = "partial ";
    private const string Async = "async ";
    private const string MethodBodyStart =
        """
            {
        """;
    private const string MethodBodyEnd =
        """
            }
        """;
    private const string OpenParenthesis = "(";
    private const string ClosedParenthesis = ")";
    private const string Space = " ";
    private const string Indent = "    ";
    private const string ParameterSeparator = ", ";

    public static void GenerateApiMethod(this StringBuilder builder, ApiMethod apiMethod, ApiClient apiClient, CancellationToken cancellationToken)
    {
        builder.AppendLine();

        builder.Append(Indent);
        builder.AppendLine(Constants.GeneratedCodeAttribute);

        builder.Append(Indent);
        builder.Append(apiMethod.AccessModifier);
        builder.Append(Space);

        if (apiMethod.ReturnType.IsTask)
        {
            builder.Append(Async);
        }

        builder.Append(Partial);
        builder.GenerateMethodReturnType(apiMethod.ReturnType);
        builder.Append(apiMethod.Name);
        builder.GenerateMethodParameters(apiMethod.Parameters, cancellationToken);

        builder.AppendLine(MethodBodyStart);
        builder.GenerateApiMethodBody(apiMethod, apiClient);
        builder.AppendLine(MethodBodyEnd);
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

        builder.Append(Space);
    }

    private static void GenerateMethodParameters(this StringBuilder builder, EquatableArray<ApiMethodParameter> parameters, CancellationToken cancellationToken)
    {
        builder.Append(OpenParenthesis);

        for (var i = 0; i < parameters.Length; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameter = parameters[i];

            builder.Append(parameter.TypeInfo.FullTypeName);
            builder.Append(Space);
            builder.Append(parameter.TypeInfo.ParameterName);

            if (i < parameters.Length - 1)
            {
                builder = builder.Append(ParameterSeparator);
            }
        }

        builder.AppendLine(ClosedParenthesis);
    }
}
