using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;

internal static class JsonConverterGenerator
{
    public static (string hint, SourceText) GenerateJsonConverters(this List<Enumeration> enums, CancellationToken cancellationToken)
    {
        var source = GenerateJsonConverterSource(enums, cancellationToken);
        var hint = $"JsonConverters.g.cs";

        return (hint, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateJsonConverterSource(this List<Enumeration> enums, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();

        builder.GenerateJsonConverterStart(enums, cancellationToken);

        foreach (var enumeration in enums)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.GenerateEnumJsonConverter(enumeration, cancellationToken);
        }

        builder.GenerateJsonConverterEnd();

        return builder.ToString();
    }

    private static void GenerateJsonConverterStart(this StringBuilder builder, List<Enumeration> enums, CancellationToken cancellationToken)
    {
        builder.AppendLine(
            $$"""
            #nullable enable

            namespace {{"DiscogsApiClient"}};
        
            internal static class {{"EnumJsonConverters"}}
            {
                public static global::System.Collections.Generic.IReadOnlyList<global::System.Text.Json.Serialization.JsonConverter> Converters { get; } = new global::System.Collections.Generic.List<global::System.Text.Json.Serialization.JsonConverter>
                {
            """);

        for (var i = 0; i < enums.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var enumeration = enums[i];
            var className = $"{enumeration.FullName.Replace(".", "")}JsonConverter";

            builder.Append($"\t\tnew {className}()");

            if (i < enums.Count - 1)
            {
                builder.AppendLine(",");
            }
            else
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine(
            """
                };
            """);
    }

    private static void GenerateJsonConverterEnd(this StringBuilder builder)
    {
        builder.AppendLine("}");
    }

    private static void GenerateEnumJsonConverter(this StringBuilder builder, Enumeration enumeration, CancellationToken cancellationToken)
    {
        var className = $"{enumeration.FullName.Replace(".", "")}JsonConverter";

        builder.AppendLine(
            $$"""


                private sealed class {{className}}
                    : global::System.Text.Json.Serialization.JsonConverter<global::{{enumeration.FullName}}>
                {
                    public override global::{{enumeration.FullName}} Read(
                        ref global::System.Text.Json.Utf8JsonReader reader,
                        global::System.Type typeToConvert,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        global::{{enumeration.FullName}} enumValue;

            """);

        for (var i = 0; i < enumeration.Members.Count; i++)
        {
            var enumMember = enumeration.Members[i];

            if (i == 0)
            {
                builder.Append($"\t\t\tif (reader.ValueTextEquals(\"{enumMember.DisplayName}\"))");
            }
            else
            {
                builder.Append($"\t\t\telse if (reader.ValueTextEquals(\"{enumMember.DisplayName}\"))");
            }

            builder.AppendLine(
                $$"""

                            {
                                enumValue = global::{{enumeration.FullName}}.{{enumMember.FieldName}};
                            }
                """);
        }

        builder.AppendLine(
            $$"""
                        else
                        {
                            throw new global::System.Text.Json.JsonException($"Value '{reader.GetString()}' can not be serialized as {typeof(global::{{enumeration.FullName}}).FullName}.");
                        }

                        return enumValue;
                    }

                    public override void Write(
                        global::System.Text.Json.Utf8JsonWriter writer,
                        global::{{enumeration.FullName}} value,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        throw new global::System.NotImplementedException("Serializing to Json is not support for '{{className}}'.");
                    }
                }
            """);
    }
}
