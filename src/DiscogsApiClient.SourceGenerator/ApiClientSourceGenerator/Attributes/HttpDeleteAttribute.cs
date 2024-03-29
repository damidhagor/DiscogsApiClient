﻿namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class HttpDeleteAttribute
{
    public const string Name = "HttpDeleteAttribute";

    public const string SourceHint = "HttpDeleteAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [global::System.AttributeUsage(global::System.AttributeTargets.Method)]
        internal sealed class {{Name}} : global::{{Constants.ApiClientNamespace}}.{{HttpMethodBaseAttribute.Name}}
        {
            public const string Method = "Delete";
        
            public {{Name}}(string route)
                : base(route)
            { }
        }
        """;
}
