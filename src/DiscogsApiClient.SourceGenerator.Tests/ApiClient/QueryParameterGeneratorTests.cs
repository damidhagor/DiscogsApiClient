using System.Collections.Immutable;
using System.Text;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;
using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class QueryParameterGeneratorTests
{
    [Test]
    public async Task ShouldGenerateQueryParameterClasses_WhenStringPropertiesPresent()
    {
        var stringProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Name",
                "name",
                new ParsedTypeInfo("String", "System", true, true, [], [])),
            QueryParameterType.String);

        var queryParameter = new ApiMethodParameter(
            new ParsedParameterTypeInfo(
                "queryParams",
                "queryParams",
                new ParsedTypeInfo("QueryParams", "TestNamespace", true, true, [], [])),
            ApiMethodParameterType.Query,
            QueryParameters: [stringProperty]);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            "public",
            [queryParameter],
            new ApiMethodReturnType(new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], [])));

        var builder = new StringBuilder();
        builder.GenerateQueryParameterClasses([apiMethod], default);
        var source = builder.ToString();

        var expected =
            """
            file static class TestNamespaceQueryParamsExtensions
            {
                public static void CalculateQuerySize(this global::TestNamespace.QueryParams? queryParams, ref int capacity, ref int parameterCount)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            capacity += 5; // Name
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Name);
                            parameterCount++;
                        }
                    }
                }

                public static void AppendQuery(this global::TestNamespace.QueryParams? queryParams, global::System.Text.StringBuilder queryBuilder, int routeLength)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("name=");
                            queryBuilder.Append(queryParams.Name);
                        }
                    }
                }
            }

            file static class QueryParameterHelper
            {
                public static int CalculateQuerySize(string? text)
                {
                    return text?.Length ?? 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateQueryParameterClasses_WhenIntPropertiesPresent()
    {
        var intProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Page",
                "page",
                new ParsedTypeInfo("Int32", "System", true, true, [], [])),
            QueryParameterType.Integer);

        var queryParameter = new ApiMethodParameter(
            new ParsedParameterTypeInfo(
                "queryParams",
                "queryParams",
                new ParsedTypeInfo("QueryParams", "TestNamespace", true, true, [], [])),
            ApiMethodParameterType.Query,
            QueryParameters: [intProperty]);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            "public",
            [queryParameter],
            new ApiMethodReturnType(new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], [])));

        var builder = new StringBuilder();
        builder.GenerateQueryParameterClasses([apiMethod], default);
        var source = builder.ToString();

        var expected =
            """
            file static class TestNamespaceQueryParamsExtensions
            {
                public static void CalculateQuerySize(this global::TestNamespace.QueryParams? queryParams, ref int capacity, ref int parameterCount)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Page is not null)
                        {
                            capacity += 5; // Page
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Page);
                            parameterCount++;
                        }
                    }
                }

                public static void AppendQuery(this global::TestNamespace.QueryParams? queryParams, global::System.Text.StringBuilder queryBuilder, int routeLength)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Page is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("page=");
                            queryBuilder.Append(queryParams.Page);
                        }
                    }
                }
            }

            file static class QueryParameterHelper
            {
                public static int CalculateQuerySize(int? number)
                {
                    return number?.ToString()?.Length ?? 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateQueryParameterClasses_WhenEnumPropertiesPresent()
    {
        var enumMembers = ImmutableArray.Create<EnumerationMember>([
            new("Ascending", "asc"),
            new("Descending", "desc")]);

        var enumProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Sort",
                "sort",
                new ParsedTypeInfo("SortOrder", "TestNamespace", true, true, [], enumMembers)),
            QueryParameterType.Enum);

        var queryParameter = new ApiMethodParameter(
            new ParsedParameterTypeInfo(
                "queryParams",
                "queryParams",
                new ParsedTypeInfo("QueryParams", "TestNamespace", true, true, [], [])),
            ApiMethodParameterType.Query,
            QueryParameters: [enumProperty]);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            "public",
            [queryParameter],
            new ApiMethodReturnType(new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], [])));

        var builder = new StringBuilder();
        builder.GenerateQueryParameterClasses([apiMethod], default);
        var source = builder.ToString();

        var expected =
            """
            file static class TestNamespaceQueryParamsExtensions
            {
                public static void CalculateQuerySize(this global::TestNamespace.QueryParams? queryParams, ref int capacity, ref int parameterCount)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Sort is not null)
                        {
                            capacity += 5; // Sort
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Sort);
                            parameterCount++;
                        }
                    }
                }

                public static void AppendQuery(this global::TestNamespace.QueryParams? queryParams, global::System.Text.StringBuilder queryBuilder, int routeLength)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Sort is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("sort=");
                            queryBuilder.Append(queryParams.Sort switch
                            {
                                global::TestNamespace.SortOrder.Ascending => "asc",
                                global::TestNamespace.SortOrder.Descending => "desc",
                                _ => throw new global::System.ArgumentOutOfRangeException(nameof(queryParams.Sort))
                            });
                        }
                    }
                }
            }

            file static class QueryParameterHelper
            {
                public static int CalculateQuerySize(global::TestNamespace.SortOrder? enumValue)
                {
                    return enumValue.HasValue
                        ? enumValue switch
                        {
                            global::TestNamespace.SortOrder.Ascending => 3, // asc
                            global::TestNamespace.SortOrder.Descending => 4, // desc
                            _ => throw new global::System.ArgumentOutOfRangeException(nameof(enumValue))
                        }
                        : 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldNotGenerateHelper_WhenNoQueryParameters()
    {
        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            "public",
            [],
            new ApiMethodReturnType(new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], [])));

        var builder = new StringBuilder();
        builder.GenerateQueryParameterClasses([apiMethod], CancellationToken.None);
        var source = builder.ToString();

        await Assert.That(source).IsEmpty();
    }

    [Test]
    public async Task ShouldGenerateQueryParameterClasses_WhenMixedPropertiesPresent()
    {
        var stringProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Name",
                "name",
                new ParsedTypeInfo("String", "System", true, true, [], [])),
            QueryParameterType.String);

        var intProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Page",
                "page",
                new ParsedTypeInfo("Int32", "System", true, true, [], [])),
            QueryParameterType.Integer);

        var enumMembers = ImmutableArray.Create(new EnumerationMember[]
        {
            new("Ascending", "asc"),
            new("Descending", "desc")
        });

        var enumProperty = new QueryParameter(
            new ParsedParameterTypeInfo(
                "Sort",
                "sort",
                new ParsedTypeInfo("SortOrder", "TestNamespace", true, true, [], enumMembers)),
            QueryParameterType.Enum);

        var queryParameter = new ApiMethodParameter(
            new ParsedParameterTypeInfo(
                "queryParams",
                "queryParams",
                new ParsedTypeInfo("QueryParams", "TestNamespace", true, true, [], [])),
            ApiMethodParameterType.Query,
            QueryParameters: [stringProperty, intProperty, enumProperty]);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            "public",
            [queryParameter],
            new ApiMethodReturnType(new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], [])));

        var builder = new StringBuilder();
        builder.GenerateQueryParameterClasses([apiMethod], CancellationToken.None);
        var source = builder.ToString();

        var expected =
            """
            file static class TestNamespaceQueryParamsExtensions
            {
                public static void CalculateQuerySize(this global::TestNamespace.QueryParams? queryParams, ref int capacity, ref int parameterCount)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            capacity += 5; // Name
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Name);
                            parameterCount++;
                        }

                        if (queryParams.Page is not null)
                        {
                            capacity += 5; // Page
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Page);
                            parameterCount++;
                        }

                        if (queryParams.Sort is not null)
                        {
                            capacity += 5; // Sort
                            capacity += QueryParameterHelper.CalculateQuerySize(queryParams.Sort);
                            parameterCount++;
                        }
                    }
                }

                public static void AppendQuery(this global::TestNamespace.QueryParams? queryParams, global::System.Text.StringBuilder queryBuilder, int routeLength)
                {
                    if (queryParams is not null)
                    {
                        if (queryParams.Name is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("name=");
                            queryBuilder.Append(queryParams.Name);
                        }

                        if (queryParams.Page is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("page=");
                            queryBuilder.Append(queryParams.Page);
                        }

                        if (queryParams.Sort is not null)
                        {
                            if (queryBuilder.Length > routeLength)
                            {
                                queryBuilder.Append('&');
                            }

                            queryBuilder.Append("sort=");
                            queryBuilder.Append(queryParams.Sort switch
                            {
                                global::TestNamespace.SortOrder.Ascending => "asc",
                                global::TestNamespace.SortOrder.Descending => "desc",
                                _ => throw new global::System.ArgumentOutOfRangeException(nameof(queryParams.Sort))
                            });
                        }
                    }
                }
            }

            file static class QueryParameterHelper
            {
                public static int CalculateQuerySize(string? text)
                {
                    return text?.Length ?? 0;
                }

                public static int CalculateQuerySize(int? number)
                {
                    return number?.ToString()?.Length ?? 0;
                }

                public static int CalculateQuerySize(global::TestNamespace.SortOrder? enumValue)
                {
                    return enumValue.HasValue
                        ? enumValue switch
                        {
                            global::TestNamespace.SortOrder.Ascending => 3, // asc
                            global::TestNamespace.SortOrder.Descending => 4, // desc
                            _ => throw new global::System.ArgumentOutOfRangeException(nameof(enumValue))
                        }
                        : 0;
                }
            }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }
}
