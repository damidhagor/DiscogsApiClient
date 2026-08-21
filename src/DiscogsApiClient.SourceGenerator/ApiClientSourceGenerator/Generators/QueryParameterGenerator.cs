using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class QueryParameterGenerator
{
    public static void GenerateQueryParameterClasses(this StringBuilder builder, EquatableArray<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        builder.GenerateQueryParameterExtensions(apiMethods, cancellationToken);
        builder.GenerateQueryParameterPropertyExtensions(apiMethods, cancellationToken);
    }

    private static void GenerateQueryParameterExtensions(this StringBuilder builder, EquatableArray<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters.Where(p => p.ParameterType == ApiMethodParameterType.Query));

        foreach (var parameter in queryParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!implementedExtensions.Add(parameter.TypeInfo.FullTypeName))
            {
                continue;
            }

            builder.AppendTopLevelSeparator();

            builder.AppendLine(
                $$"""
                {{Constants.GeneratedCodeAttribute}}
                file static class {{parameter.TypeInfo.Namespace.Replace(".", "")}}{{parameter.TypeInfo.Name}}Extensions
                {
                    public static void CalculateQuerySize(this {{parameter.TypeInfo.FullTypeName}} {{parameter.TypeInfo.ParameterName}}, ref int capacity, ref int parameterCount)
                    {
                        if ({{parameter.TypeInfo.ParameterName}} is not null)
                        {
                """);

            var isFirstProperty = true;
            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!isFirstProperty)
                {
                    builder.AppendLine();
                }

                isFirstProperty = false;

                builder.AppendLine(
                    $$"""
                                    if ({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}} is not null)
                                    {
                                        capacity += {{property.TypeInfo.ParameterName.Length + 1}}; // {{property.TypeInfo.ParameterName}}
                                        capacity += QueryParameterHelper.CalculateQuerySize({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}});
                                        parameterCount++;
                                    }
                        """);
            }

            builder.AppendLine(
                """
                        }
                    }
                """);

            builder.AppendLine(
                $$"""

                    public static void AppendQuery(this {{parameter.TypeInfo.FullTypeName}} {{parameter.TypeInfo.ParameterName}}, global::System.Text.StringBuilder queryBuilder, int routeLength)
                    {
                        if ({{parameter.TypeInfo.ParameterName}} is not null)
                        {
                """);

            isFirstProperty = true;
            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!isFirstProperty)
                {
                    builder.AppendLine();
                }

                isFirstProperty = false;

                builder.AppendLine(
                    $$"""
                                if ({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}} is not null)
                                {
                                    if (queryBuilder.Length > routeLength)
                                    {
                                        queryBuilder.Append('&');
                                    }

                                    queryBuilder.Append("{{property.TypeInfo.ParameterNameAlias}}=");
                    """);

                if (property.ParameterType == QueryParameterType.Enum)
                {
                    builder.AppendLine(
                        $$"""
                                        queryBuilder.Append({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}} switch
                                        {
                        """);

                    foreach (var enumMember in property.TypeInfo.EnumMembers!)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        builder.AppendLine(
                            $$"""
                                                {{property.TypeInfo.GetFullTypeName(false)}}.{{enumMember.MemberName}} => "{{enumMember.MemberNameAlias}}",
                            """);
                    }

                    builder.AppendLine(
                        $$"""
                                            _ => throw new global::System.ArgumentOutOfRangeException(nameof({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}}))
                        """);

                    builder.AppendLine(
                        """
                                        });
                        """);
                }
                else if (property.ParameterType == QueryParameterType.String)
                {
                    builder.AppendLine(
                        $$"""
                                        queryBuilder.Append(global::System.Uri.EscapeDataString({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}}));
                        """);
                }
                else
                {
                    builder.AppendLine(
                        $$"""
                                        queryBuilder.Append({{parameter.TypeInfo.ParameterName}}.{{property.TypeInfo.ParameterName}});
                        """);
                }

                builder.AppendLine(
                    """
                                }
                    """);
            }

            builder.AppendLine(
                """
                        }
                    }
                """);

            builder.Append("}");
        }
    }

    private static void GenerateQueryParameterPropertyExtensions(this StringBuilder builder, EquatableArray<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters.Where(p => p.ParameterType == ApiMethodParameterType.Query))
            .SelectMany(p => p.QueryParameters)
            .Where(p => implementedExtensions.Add(p.TypeInfo.FullTypeName))
            .ToArray();

        if (queryParameters.Length == 0)
        {
            return;
        }

        builder.AppendTopLevelSeparator();

        builder.AppendLine(
            $$"""
            {{Constants.GeneratedCodeAttribute}}
            file static class QueryParameterHelper
            {
            """);

        var isFirstMethod = true;
        foreach (var parameter in queryParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!isFirstMethod)
            {
                builder.AppendLine();
            }

            isFirstMethod = false;

            builder.GenerateQueryParameterHelperMethod(parameter, cancellationToken);
        }

        builder.Append("}");
    }

    private static void GenerateQueryParameterHelperMethod(this StringBuilder builder, QueryParameter parameter, CancellationToken cancellationToken)
    {
        switch (parameter.ParameterType)
        {
            case QueryParameterType.String:
                builder.AppendLine(
                    """
                        public static int CalculateQuerySize(string? text)
                        {
                            if (text is null)
                            {
                                return 0;
                            }

                            var size = 0;

                            foreach (var c in text)
                            {
                                size += IsUnreservedQueryCharacter(c) ? 1 : 3;
                            }

                            return size;
                        }

                        private static bool IsUnreservedQueryCharacter(char c)
                            => c is >= 'A' and <= 'Z'
                                or >= 'a' and <= 'z'
                                or >= '0' and <= '9'
                                or '-' or '_' or '.' or '~';
                    """);
                break;

            case QueryParameterType.Integer:
                builder.AppendLine(
                    """
                        public static int CalculateQuerySize(int? number)
                        {
                            return number?.ToString()?.Length ?? 0;
                        }
                    """);
                break;

            case QueryParameterType.Enum:
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize({{parameter.TypeInfo.FullTypeName}} enumValue)
                        {
                            return enumValue switch
                            {
                    """);

                if (parameter.TypeInfo.IsNullable)
                {
                    builder.AppendLine(
                        """
                                    null => 0,
                        """);
                }

                foreach (var enumMember in parameter.TypeInfo.EnumMembers)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    builder.AppendLine(
                        $$"""
                                    {{parameter.TypeInfo.GetFullTypeName(false)}}.{{enumMember.MemberName}} => {{enumMember.MemberNameAlias.Length}}, // {{enumMember.MemberNameAlias}}
                        """);
                }

                builder.AppendLine(
                    """
                                _ => throw new global::System.ArgumentOutOfRangeException(nameof(enumValue))
                            };
                        }
                    """);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(parameter));
        }
    }

    private static void AppendTopLevelSeparator(this StringBuilder builder)
    {
        if (builder.Length == 0)
        {
            return;
        }

        builder.AppendLine();
        builder.AppendLine();
    }
}
