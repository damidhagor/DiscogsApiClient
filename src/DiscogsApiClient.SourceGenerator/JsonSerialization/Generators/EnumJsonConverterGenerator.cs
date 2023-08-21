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
                    : global::System.Text.Json.Serialization.JsonConverter<{{enumeration.TypeInfo.FullTypeName}}>
                {
                    public override {{enumeration.TypeInfo.FullTypeName}} Read(
                        ref global::System.Text.Json.Utf8JsonReader reader,
                        global::System.Type typeToConvert,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        {{enumeration.TypeInfo.FullTypeName}} enumValue;

            """);

        for (var i = 0; i < enumeration.TypeInfo.EnumMembers.Count; i++)
        {
            var enumMember = enumeration.TypeInfo.EnumMembers[i];

            if (i == 0)
            {
                builder.Append($"\t\t\tif (reader.ValueTextEquals(\"{enumMember.MemberNameAlias}\"))");
            }
            else
            {
                builder.Append($"\t\t\telse if (reader.ValueTextEquals(\"{enumMember.MemberNameAlias}\"))");
            }

            builder.AppendLine(
                $$"""

                            {
                                enumValue = {{enumeration.TypeInfo.GetFullTypeName(false)}}.{{enumMember.MemberName}};
                            }
                """);
        }

        builder.AppendLine(
            $$"""
                        else
                        {
                            throw new global::System.Text.Json.JsonException($"Value '{reader.GetString()}' can not be serialized as '{typeof({{enumeration.TypeInfo.GetFullTypeName(false)}}).FullName}'.");
                        }

                        return enumValue;
                    }

                    public override void Write(
                        global::System.Text.Json.Utf8JsonWriter writer,
                        {{enumeration.TypeInfo.FullTypeName}} value,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        throw new global::System.NotImplementedException($"Serializing to Json is not supported for '{typeof({{enumeration.TypeInfo.GetFullTypeName(false)}}).FullName}'.");
                    }
                }
            """);
    }

    private static string GetJsonConverterClassName(this Enumeration enumeration)
        => $"{enumeration.TypeInfo.Namespace.Replace(".", "")}{enumeration.TypeInfo.Name}JsonConverter";
}
