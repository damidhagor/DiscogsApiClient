namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiClientSettingsGenerator
{
    public const string Namespace = Constants.ApiClientNamespace;

    public const string Name = "ApiClientSettings";

    public const string SourceHint = "ApiClientSettings.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Namespace}};

        internal sealed class {{Name}}<T, U>
            where U : global::System.Text.Json.Serialization.JsonSerializerContext
        {
            public U JsonSerializerContext { get; init; }

            public {{Name}}(U jsonSerializerContext)
            {
                JsonSerializerContext = jsonSerializerContext;
            }
        }
        """;

    public static IncrementalGeneratorPostInitializationContext AddApiClientSettings(this IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource(SourceHint, CreateSourceText(Source));
        return context;
    }

    private static SourceText CreateSourceText(string source)
        => SourceText.From(source, Encoding.UTF8);
}
