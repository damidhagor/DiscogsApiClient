namespace DiscogsApiClient.ApiClientGenerator.Generators;

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
        builder.GenerateMethodParameters(apiMethod.Parameters);

        builder.AppendLine(_methodBodyStart);
        builder.GenerateApiMethodBody(apiMethod, cancellationToken);
        builder.AppendLine(_methodBodyEnd);
    }

    private static void GenerateMethodReturnType(this StringBuilder builder, ApiMethodReturnType returnType)
    {
        if (returnType.IsTask)
        {
            builder.Append(_async);
        }

        builder.Append(returnType.FullName);
        builder.Append(_space);
    }

    private static void GenerateMethodParameters(this StringBuilder builder, List<ApiMethodParameter> parameters)
    {
        builder.Append(_openParenthesis);

        for (var i = 0; i < parameters.Count; i++)
        {
            var parameter = parameters[i];

            builder.Append(parameter.FullName);

            if (i < parameters.Count - 1)
            {
                builder = builder.Append(_parameterSeperator);
            }
        }

        builder.AppendLine(_closedParenthesis);
    }
}
