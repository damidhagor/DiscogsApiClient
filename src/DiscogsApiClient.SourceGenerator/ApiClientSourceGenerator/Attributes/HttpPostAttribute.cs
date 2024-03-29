﻿namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpPostAttribute
{
    public const string Name = "HttpPostAttribute";

    public const string SourceHint = "HttpPostAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal sealed class {{Name}} : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Post";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
