using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;

namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

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

            if (implementedExtensions.Contains(parameter.TypeInfo.FullTypeName))
            {
                continue;
            }

            builder.AppendLine(
                $$"""
                

                #if NET7_0_OR_GREATER
                file static class {{parameter.TypeInfo.Namespace.Replace(".", "")}}{{parameter.TypeInfo.Name}}Extensions
                #else
                internal static class {{parameter.TypeInfo.Namespace.Replace(".", "")}}{{parameter.TypeInfo.Name}}Extensions
                #endif
                {
                    public static void CalculateQuerySize(this {{parameter.TypeInfo.FullTypeName}} {{parameter.TypeInfo.ParameterName}}, ref int capacity, ref int parameterCount)
                    {
                        if ({{parameter.TypeInfo.ParameterName}} is not null)
                        {
                """);

            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

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

            foreach (var property in parameter.QueryParameters)
            {
                cancellationToken.ThrowIfCancellationRequested();

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


            builder.AppendLine("}");

            implementedExtensions.Add(parameter.TypeInfo.FullTypeName);
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

            if (implementedExtensions.Contains(parameter.TypeInfo.FullTypeName))
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
                        public static int CalculateQuerySize({{parameter.TypeInfo.FullTypeName}} enumValue)
                        {
                            return enumValue.HasValue
                                ? enumValue switch
                                {
                    """);

                if (parameter.TypeInfo.EnumMembers is not null)
                {
                    foreach (var enumMember in parameter.TypeInfo.EnumMembers)
                    {
                        builder.AppendLine(
                            $$"""
                                         {{parameter.TypeInfo.GetFullTypeName(false)}}.{{enumMember.MemberName}} => {{enumMember.MemberNameAlias.Length}}, // {{enumMember.MemberNameAlias}}
                            """);
                    }
                }

                builder.AppendLine(
                    $$"""
                                    _ => throw new global::System.ArgumentOutOfRangeException(nameof(enumValue))
                                }
                            : 0;
                        }
                    """);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(parameter.ParameterType));
            }

            implementedExtensions.Add(parameter.TypeInfo.FullTypeName);
        }

        builder.AppendLine("}");
    }
}
