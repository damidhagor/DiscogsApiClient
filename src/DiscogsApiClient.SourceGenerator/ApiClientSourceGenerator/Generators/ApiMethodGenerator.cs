using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiMethodGenerator
{
    private const string _methodStart =
        """
            public 
        """;
    private const string _methodBodyStart =
        """
            {
        """;
    private const string _methodBodyEnd =
        """
            }
        """;
    private const string _async = "async ";
    private const string _openParenthesis = "(";
    private const string _closedParenthesis = ")";
    private const string _space = " ";
    private const string _parameterSeperator = ", ";

    public static void GenerateApiMethod(this StringBuilder builder, ApiMethod apiMethod, CancellationToken cancellationToken)
    {
        builder.AppendLine();

        builder.Append(_methodStart);
        builder.GenerateMethodReturnType(apiMethod.ReturnType);
        builder.Append(apiMethod.Name);
        builder.GenerateMethodParameters(apiMethod.Parameters, cancellationToken);

        builder.AppendLine(_methodBodyStart);
        builder.GenerateApiMethodBody(apiMethod);
        builder.AppendLine(_methodBodyEnd);
    }

    private static void GenerateMethodReturnType(this StringBuilder builder, ApiMethodReturnType returnType)
    {
        if (returnType.IsTask)
        {
            builder.Append(_async);
        }

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

    private static void GenerateMethodParameters(this StringBuilder builder, List<ApiMethodParameter> parameters, CancellationToken cancellationToken)
    {
        builder.Append(_openParenthesis);

        for (var i = 0; i < parameters.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameter = parameters[i];

            builder.Append(parameter.TypeInfo.FullTypeName);
            builder.Append(_space);
            builder.Append(parameter.TypeInfo.ParameterName);

            if (i < parameters.Count - 1)
            {
                builder = builder.Append(_parameterSeperator);
            }
        }

        builder.AppendLine(_closedParenthesis);
    }
}
