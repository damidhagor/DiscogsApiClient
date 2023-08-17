﻿namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class HttpPutAttribute
{
    public const string Name = "HttpPutAttribute";

    public const string SourceHint = "HttpPutAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.GeneratorNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Method)]
        public sealed class {{Name}} : global::{{HttpMethodBaseAttribute.Namespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Put";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
