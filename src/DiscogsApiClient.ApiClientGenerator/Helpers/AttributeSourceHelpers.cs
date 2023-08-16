namespace DiscogsApiClient.ApiClientGenerator.Helpers;

internal static class AttributeSourceHelpers
{
    public const string AttributesNamespace = "DiscogsApiClient.ApiClientGenerator";

    public const string GenerateApiClientAttributeName = "ApiClientAttribute";
    private const string _generateApiClientAttributeHint = "ApiClientAttribute.g.cs";
    private const string _generateApiClientAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Interface)]
        public sealed class {{GenerateApiClientAttributeName}} : System.Attribute { }
        """;

    public const string HttpMethodAttributeName = "HttpMethodAttribute";
    private const string _httpMethodAttributeHint = "HttpMethodAttribute.g.cs";
    private const string _httpMethodAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public abstract class {{HttpMethodAttributeName}} : System.Attribute
        {
            public System.Net.Http.HttpMethod Method { get; set; }
            public string Route { get; set; }
        
            public {{HttpMethodAttributeName}}(System.Net.Http.HttpMethod method, string route)
            {
                Method = method;
                Route = route;
            }
        }
        """;

    public const string HttpGetAttributeName = "HttpGetAttribute";
    private const string _httpGetAttributeHint = "HttpGetAttribute.g.cs";
    private const string _httpGetAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{HttpGetAttributeName}} : global::{{AttributesNamespace}}.{{HttpMethodAttributeName}}
        {
            public {{HttpGetAttributeName}}(string route)
                : base(System.Net.Http.HttpMethod.Get, route)
            { }
        }
        """;

    public const string HttpPostAttributeName = "HttpPostAttribute";
    private const string _httpPostAttributeHint = "HttpPostAttribute.g.cs";
    private const string _httpPostAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{HttpPostAttributeName}} : global::{{AttributesNamespace}}.{{HttpMethodAttributeName}}
        {
            public {{HttpPostAttributeName}}(string route)
                : base(System.Net.Http.HttpMethod.Get, route)
            { }
        }
        """;

    public const string HttpPutAttributeName = "HttpPutAttribute";
    private const string _httpPutAttributeHint = "HttpPutAttribute.g.cs";
    private const string _httpPutAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{HttpPutAttributeName}} : global::{{AttributesNamespace}}.{{HttpMethodAttributeName}}
        {
            public {{HttpPutAttributeName}}(string route)
                : base(System.Net.Http.HttpMethod.Get, route)
            { }
        }
        """;

    public const string HttpDeleteAttributeName = "HttpDeleteAttribute";
    private const string _httpDeleteAttributeHint = "HttpDeleteAttribute.g.cs";
    private const string _httpDeleteAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{HttpDeleteAttributeName}} : global::{{AttributesNamespace}}.{{HttpMethodAttributeName}}
        {
            public {{HttpDeleteAttributeName}}(string route)
                : base(System.Net.Http.HttpMethod.Get, route)
            { }
        }
        """;

    public const string BodyAttributeName = "BodyAttribute";
    private const string _bodyAttributeHint = "BodyAttribute.g.cs";
    private const string _bodyAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Parameter)]
        public sealed class {{BodyAttributeName}} : System.Attribute { }
        """;

    public const string AliasAsAttributeName = "AliasAsAttribute";
    private const string _aliasAsAttributeHint = "AliasAsAttribute.g.cs";
    private const string _aliasAsAttributeSource =
        $$"""
        namespace {{AttributesNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
        public sealed class {{AliasAsAttributeName}} : System.Attribute
        {
            public string Alias { get; set; }
        
            public {{AliasAsAttributeName}}(string alias)
            {
                Alias = alias;
            }
        }
        """;

    public static IncrementalGeneratorPostInitializationContext AddApiClientAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_generateApiClientAttributeHint, CreateSourceText(_generateApiClientAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpMethodAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_httpMethodAttributeHint, CreateSourceText(_httpMethodAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpGetAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_httpGetAttributeHint, CreateSourceText(_httpGetAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpPostAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_httpPostAttributeHint, CreateSourceText(_httpPostAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpPutAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_httpPutAttributeHint, CreateSourceText(_httpPutAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddHttpDeleteAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_httpDeleteAttributeHint, CreateSourceText(_httpDeleteAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddBodyAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_bodyAttributeHint, CreateSourceText(_bodyAttributeSource));
        return context;
    }

    public static IncrementalGeneratorPostInitializationContext AddAliasAsAttribute(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(_aliasAsAttributeHint, CreateSourceText(_aliasAsAttributeSource));
        return context;
    }

    private static SourceText CreateSourceText(string source)
        => SourceText.From(source, Encoding.UTF8);
}
