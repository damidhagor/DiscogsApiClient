using DiscogsApiClient.ApiClientGenerator.Models.MethodParameters;

namespace DiscogsApiClient.ApiClientGenerator.Generators;

internal static class QueryParameterGenerator
{
    public static void GenerateQueryParameterClasses(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        builder.GenerateQueryParameterExtensions(apiMethods, cancellationToken);
        builder.GenerateQueryParameterPropertyExtensions(apiMethods, cancellationToken);
    }

    private static void GenerateQueryParameterExtensions(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters)
            .OfType<QueryApiMethodParameter>();

        foreach (var parameter in queryParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (implementedExtensions.Contains(parameter.FullName))
            {
                continue;
            }

            builder.AppendLine(
                $$"""
                

                #if NET7_0_OR_GREATER
                file static class {{parameter.TypeFullName.Replace(".", "").Replace("?", "")}}Extensions
                #else
                internal static class {{parameter.TypeFullName.Replace(".", "").Replace("?", "")}}Extensions
                #endif
                {
                    public static void CalculateQuerySize(this {{parameter.FullName}}, ref int capacity, ref int parameterCount)
                    {
                        if ({{parameter.Name}} is not null)
                        {
                """);

            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

                builder.AppendLine(
                    $$"""
                                    if ({{parameter.Name}}.{{property.PropertyName}} is not null)
                                    {
                                        capacity += {{property.ParameterName.Length + 1}}; // {{property.ParameterName}}
                                        capacity += QueryParameterHelper.CalculateQuerySize({{parameter.Name}}.{{property.PropertyName}});
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

                    public static void AppendQuery(this {{parameter.FullName}}, System.Text.StringBuilder queryBuilder, int routeLength)
                    {
                        if ({{parameter.Name}} is not null)
                        {
                """);

            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

                builder.AppendLine(
                    $$"""
                                if ({{parameter.Name}}.{{property.PropertyName}} is not null)
                                {
                                    if (queryBuilder.Length > routeLength)
                                    {
                                        queryBuilder.Append('&');
                                    }

                                    queryBuilder.Append("{{property.ParameterName}}=");
                    """);

                if (property.ParameterType == QueryParameterType.Enum)
                {
                    builder.AppendLine(
                        $$"""
                                        queryBuilder.Append({{parameter.Name}} switch
                                        {
                        """);

                    foreach (var enumValue in property.EnumValues!)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        builder.AppendLine(
                            $$"""
                                                { {{property.PropertyName}}: {{property.PropertyType}}.{{enumValue.MemberName}} } => "{{enumValue.DisplayName}}",
                            """);
                    }

                    builder.AppendLine(
                        """
                                        });
                        """);
                }
                else
                {
                    builder.AppendLine(
                        $$"""
                                        queryBuilder.Append({{parameter.Name}}.{{property.PropertyName}});
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


            builder.AppendLine("}");

            implementedExtensions.Add(parameter.FullName);
        }
    }

    private static void GenerateQueryParameterPropertyExtensions(this StringBuilder builder, List<ApiMethod> apiMethods, CancellationToken cancellationToken)
    {
        var implementedExtensions = new HashSet<string>();
        var queryParameters = apiMethods
            .SelectMany(m => m.Parameters)
            .OfType<QueryApiMethodParameter>()
            .SelectMany(p => p.QueryParameters);

        builder.AppendLine(
            $$"""
                

            #if NET7_0_OR_GREATER
            file static class QueryParameterHelper
            #else
            internal static class QueryParameterHelper
            #endif
            {
            """
        );

        foreach (var parameter in queryParameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (implementedExtensions.Contains(parameter.PropertyType))
            {
                continue;
            }

            if (parameter.ParameterType == QueryParameterType.String)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize(string? text)
                        {
                            return text?.Length ?? 0;
                        }
                    """);
            }
            else if (parameter.ParameterType == QueryParameterType.Integer)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize(int? number)
                        {
                            return number?.ToString()?.Length ?? 0;
                        }
                    """);
            }
            else if (parameter.ParameterType == QueryParameterType.Enum)
            {
                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize({{parameter.PropertyType}} enumValue)
                        {
                            return enumValue switch
                            {
                    """);

                if (parameter.EnumValues is not null)
                {
                    foreach (var enumValue in parameter.EnumValues)
                    {
                        builder.AppendLine(
                            $$"""
                                        {{parameter.PropertyType}}.{{enumValue.MemberName}} => {{enumValue.DisplayName.Length}}, // {{enumValue.DisplayName}}
                            """);
                    }
                }

                builder.AppendLine(
                    $$"""
                                _ => throw new ArgumentOutOfRangeException(nameof(enumValue))
                            };
                        }
                    """);

                builder.AppendLine(
                    $$"""
                        public static int CalculateQuerySize({{parameter.PropertyType}}? enumValue)
                        {
                            return enumValue is not null ? CalculateQuerySize(enumValue.Value) : 0;
                        }
                    """);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(parameter.ParameterType));
            }

            implementedExtensions.Add(parameter.PropertyType);
        }

        builder.AppendLine("}");
    }
}
