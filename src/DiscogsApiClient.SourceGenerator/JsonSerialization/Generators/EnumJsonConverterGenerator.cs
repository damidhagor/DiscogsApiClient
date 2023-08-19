using DiscogsApiClient.SourceGenerator.JsonSerialization.Models;

namespace DiscogsApiClient.SourceGenerator.JsonSerialization.Generators;

internal static class EnumJsonConverterGenerator
{
    public const string Namespace = Constants.JsonSerializationNamespace;

    public const string ClassName = "EnumJsonConverters";

    public const string SourceHint = $"{ClassName}.g.cs";

    public static (string hint, SourceText) GenerateEnumJsonConverters(this List<Enumeration> enums, CancellationToken cancellationToken)
    {
        var source = GenerateEnumJsonConvertersSource(enums, cancellationToken);
        var hint = SourceHint;

        return (hint, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateEnumJsonConvertersSource(this List<Enumeration> enums, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();

        builder.GenerateEnumJsonConvertersStart(enums, cancellationToken);

        foreach (var enumeration in enums)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.GenerateEnumJsonConverter(enumeration, cancellationToken);
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    private static void GenerateEnumJsonConvertersStart(this StringBuilder builder, List<Enumeration> enums, CancellationToken cancellationToken)
    {
        builder.AppendLine(
            $$"""
            #nullable enable

            namespace {{Namespace}};
        
            internal static class {{ClassName}}
            {
                public static global::System.Collections.Generic.IReadOnlyList<global::System.Text.Json.Serialization.JsonConverter> Converters { get; } = new global::System.Collections.Generic.List<global::System.Text.Json.Serialization.JsonConverter>
                {
            """);

        for (var i = 0; i < enums.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.AppendLine($"\t\tnew {enums[i].GetJsonConverterClassName()}(),");
        }

        builder.AppendLine(
            """
                };
            """);
    }

    private static void GenerateEnumJsonConverter(this StringBuilder builder, Enumeration enumeration, CancellationToken cancellationToken)
    {
        var className = enumeration.GetJsonConverterClassName();

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
                        throw new global::System.NotImplementedException("Serializing to Json is not supported for '{{className}}'.");
                    }
                }
            """);
    }

    private static string GetJsonConverterClassName(this Enumeration enumeration)
        => $"{enumeration.FullName.Replace(".", "")}JsonConverter";
}
