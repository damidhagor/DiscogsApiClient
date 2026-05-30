using System.Text;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models;
using DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Models.MethodParameters;
using DiscogsApiClient.SourceGenerator.Shared.Models;

namespace DiscogsApiClient.SourceGenerator.Tests.ApiClient;

public sealed class ApiMethodGeneratorTests
{
    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsGet()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items/{id}",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsPost()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "CreateItemAsync",
            "/items/{id}",
            ApiMethodType.Post,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task CreateItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Post, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsPut()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "UpdateItemAsync",
            "/items/{id}",
            ApiMethodType.Put,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task UpdateItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Put, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsDelete()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "DeleteItemAsync",
            "/items/{id}",
            ApiMethodType.Delete,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task DeleteItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Delete, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasRouteParameters()
    {
        var idParamInfo = new ParsedParameterTypeInfo(
            "id",
            "id",
            new ParsedTypeInfo("Int32", "System", true, false, [], []));

        var routeParam = new ApiMethodParameter(
            idParamInfo,
            ApiMethodParameterType.Route,
            RoutePart: "{id}");

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var stringType = new ParsedTypeInfo("String", "System", true, false, [], []);
        var taskType = new ParsedTypeInfo(
            "Task",
            "System.Threading.Tasks",
            true,
            false,
            [stringType],
            []);

        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items/{id}",
            ApiMethodType.Get,
            [routeParam, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetItemAsync(global::System.Int32 id, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items/{id}";
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasMultipleRouteParameters()
    {
        var userIdParamInfo = new ParsedParameterTypeInfo(
            "userId",
            "userId",
            new ParsedTypeInfo("String", "System", true, false, [], []));

        var userRouteParam = new ApiMethodParameter(
            userIdParamInfo,
            ApiMethodParameterType.Route,
            RoutePart: "{userId}");

        var itemIdParamInfo = new ParsedParameterTypeInfo(
            "itemId",
            "itemId",
            new ParsedTypeInfo("Int32", "System", true, false, [], []));

        var itemRouteParam = new ApiMethodParameter(
            itemIdParamInfo,
            ApiMethodParameterType.Route,
            RoutePart: "{itemId}");

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var stringType = new ParsedTypeInfo("String", "System", true, false, [], []);
        var taskType = new ParsedTypeInfo(
            "Task",
            "System.Threading.Tasks",
            true,
            false,
            System.Collections.Immutable.ImmutableArray.Create(stringType),
            []);

        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetUserItemAsync",
            "/users/{userId}/items/{itemId}",
            ApiMethodType.Get,
            [userRouteParam, itemRouteParam, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetUserItemAsync(global::System.String userId, global::System.Int32 itemId, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/users/{userId}/items/{itemId}";
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasQueryParameters()
    {
        var queryParamsInfo = new ParsedParameterTypeInfo(
            "queryParams",
            "queryParams",
            new ParsedTypeInfo("QueryParams", "TestNamespace", true, false, [], []));

        var queryParam = new ApiMethodParameter(
            queryParamsInfo,
            ApiMethodParameterType.Query);

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var stringType = new ParsedTypeInfo("String", "System", true, false, [], []);
        var taskType = new ParsedTypeInfo(
            "Task",
            "System.Threading.Tasks",
            true,
            false,
            [stringType],
            []);

        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemsAsync",
            "/items",
            ApiMethodType.Get,
            [queryParam, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetItemsAsync(global::TestNamespace.QueryParams queryParams, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = BuildRouteForGetItemsAsync($"/items", queryParams);
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasMultipleQueryParameters()
    {
        var queryParams1Info = new ParsedParameterTypeInfo(
            "queryParams1",
            "queryParams1",
            new ParsedTypeInfo("QueryParams1", "TestNamespace", true, false, [], []));

        var queryParam1 = new ApiMethodParameter(
            queryParams1Info,
            ApiMethodParameterType.Query);

        var queryParams2Info = new ParsedParameterTypeInfo(
            "queryParams2",
            "queryParams2",
            new ParsedTypeInfo("QueryParams2", "TestNamespace", true, false, [], []));

        var queryParam2 = new ApiMethodParameter(
            queryParams2Info,
            ApiMethodParameterType.Query);

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemsAsync",
            "/items",
            ApiMethodType.Get,
            [queryParam1, queryParam2, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemsAsync(global::TestNamespace.QueryParams1 queryParams1, global::TestNamespace.QueryParams2 queryParams2, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = BuildRouteForGetItemsAsync($"/items", queryParams1, queryParams2);
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasMixedRouteAndQueryParameters()
    {
        var idParamInfo = new ParsedParameterTypeInfo(
            "id",
            "id",
            new ParsedTypeInfo("Int32", "System", true, false, [], []));

        var routeParam = new ApiMethodParameter(
            idParamInfo,
            ApiMethodParameterType.Route,
            RoutePart: "{id}");

        var queryParamsInfo = new ParsedParameterTypeInfo(
            "queryParams",
            "queryParams",
            new ParsedTypeInfo("QueryParams", "TestNamespace", true, false, [], []));

        var queryParam = new ApiMethodParameter(
            queryParamsInfo,
            ApiMethodParameterType.Query);

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemWithQueryAsync",
            "/items/{id}",
            ApiMethodType.Get,
            [routeParam, queryParam, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemWithQueryAsync(global::System.Int32 id, global::TestNamespace.QueryParams queryParams, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = BuildRouteForGetItemWithQueryAsync($"/items/{id}", queryParams);
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasBodyParameter()
    {
        var requestParamInfo = new ParsedParameterTypeInfo(
            "request",
            "request",
            new ParsedTypeInfo("TestRequest", "TestNamespace", true, false, [], []));

        var bodyParam = new ApiMethodParameter(
            requestParamInfo,
            ApiMethodParameterType.Body);

        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "CreateItemAsync",
            "/items",
            ApiMethodType.Post,
            [bodyParam, cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task CreateItemAsync(global::TestNamespace.TestRequest request, global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
                    var content = SerializeContent(request, _apiClientSettings.JsonSerializerContext.TestRequest);
            		await SendAsync(global::System.Net.Http.HttpMethod.Post, route, content: content, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasCancellationToken()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodHasNoCancellationToken()
    {
        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            [],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemAsync()
                {
            		var route = $"/items";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }


    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsAsyncWithoutResult()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var taskType = new ParsedTypeInfo("Task", "System.Threading.Tasks", true, false, [], []);
        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task GetItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
            		await SendAsync(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsAsyncWithResult()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var stringType = new ParsedTypeInfo("String", "System", true, false, [], []);
        var taskType = new ParsedTypeInfo(
            "Task",
            "System.Threading.Tasks",
            true,
            false,
            System.Collections.Immutable.ImmutableArray.Create(stringType),
            []);

        var returnType = new ApiMethodReturnType(taskType);

        var apiMethod = new ApiMethod(
            "GetItemAsync",
            "/items",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public async global::System.Threading.Tasks.Task<global::System.String> GetItemAsync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
            		var result = await SendAsync<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
            
            		return result;
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsSynchronousWithoutResult()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var voidType = new ParsedTypeInfo("Void", "System", true, false, [], []);
        var returnType = new ApiMethodReturnType(voidType);

        var apiMethod = new ApiMethod(
            "GetItemSync",
            "/items",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public void GetItemSync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
            		Send(global::System.Net.Http.HttpMethod.Get, route, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }

    [Test]
    public async Task ShouldGenerateMethodBody_WhenMethodIsSynchronousWithResult()
    {
        var cancellationTokenParamInfo = new ParsedParameterTypeInfo(
            "cancellationToken",
            "cancellationToken",
            new ParsedTypeInfo("CancellationToken", "System.Threading", true, false, [], []));

        var cancellationTokenParam = new ApiMethodParameter(
            cancellationTokenParamInfo,
            ApiMethodParameterType.CancellationToken);

        var stringType = new ParsedTypeInfo("String", "System", true, false, [], []);
        var returnType = new ApiMethodReturnType(stringType);

        var apiMethod = new ApiMethod(
            "GetItemSync",
            "/items",
            ApiMethodType.Get,
            [cancellationTokenParam],
            returnType);

        var builder = new StringBuilder();
        builder.GenerateApiMethod(apiMethod, default);
        var source = builder.ToString();

        var expected =
            """
            
                public global::System.String GetItemSync(global::System.Threading.CancellationToken cancellationToken)
                {
            		var route = $"/items";
            
            		Send<global::System.String>(global::System.Net.Http.HttpMethod.Get, route, _apiClientSettings.JsonSerializerContext.String, cancellationToken: cancellationToken);
                }
            """;

        await Assert.That(GeneratorTestHelper.NormalizeSource(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeSource(expected));
    }
}
