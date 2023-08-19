﻿namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Attributes;

internal static class ApiCLientAttribute
{
    public const string Name = "ApiClientAttribute";

    public const string SourceHint = "ApiClientAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable
        
        namespace {{Constants.ApiClientNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Interface)]
        internal sealed class {{Name}} : System.Attribute { }
        """;
}
