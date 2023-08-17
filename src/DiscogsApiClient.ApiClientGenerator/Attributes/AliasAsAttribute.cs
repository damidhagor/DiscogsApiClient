﻿namespace DiscogsApiClient.ApiClientGenerator.Attributes;

internal static class AliasAsAttribute
{
    public const string Name = "AliasAsAttribute";

    public const string SourceHint = "AliasAsAttribute.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Constants.GeneratorNamespace}};

        [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
        public sealed class {{Name}} : System.Attribute
        {
            public string Alias { get; set; }
        
            public {{Name}}(string alias)
            {
                Alias = alias;
            }
        }
        """;
}
