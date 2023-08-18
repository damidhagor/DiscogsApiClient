namespace DiscogsApiClient.SourceGenerator.ApiClientSourceGenerator.Generators;

internal static class ApiClientSettingsGenerator
{
    public const string Namespace = "DiscogsApiClient.ApiClientGenerator";

    public const string Name = "ApiClientSettings";

    public const string SourceHint = "ApiClientSettings.g.cs";

    public const string Source =
        $$"""
        #nullable enable

        namespace {{Namespace}};

        internal sealed class {{Name}}<T>
        {
            public global::System.Text.Json.JsonSerializerOptions JsonSerializerOptions { get; init; } = new();
        }

        internal static class {{Name}}Extensions
        {
            public static {{Name}}<T> AddGeneratedJsonConverters<T>(this {{Name}}<T> settings)
            {
                foreach (var jsonConverter in {{"DiscogsApiClient"}}.EnumJsonConverters.Converters)
                {
                    settings.JsonSerializerOptions.Converters.Add(jsonConverter);
                }

                return settings;
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
