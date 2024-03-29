﻿namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpGetAttribute
{
    public const string Name = "HttpGetAttribute";

    public const string SourceHint = "HttpGetAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal sealed class {{Name}} : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Get";

            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
