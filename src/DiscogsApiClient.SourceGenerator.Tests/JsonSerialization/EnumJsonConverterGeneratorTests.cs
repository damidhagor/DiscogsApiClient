using DiscogsApiClient.SourceGenerator.JsonSerialization;
using DiscogsApiClient.SourceGenerator.Shared.Attributes;
using DiscogsApiClient.SourceGenerator.Tests.JsonSerialization.Sources;

namespace DiscogsApiClient.SourceGenerator.Tests.JsonSerialization;

public sealed class EnumJsonConverterGeneratorTests
{
    [Test]
    public async Task ShouldGenerateExpectedConverterOutput_WhenMinimalEnum()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            EnumJsonConverterGeneratorTestsSources.WhenMinimalEnum);

        await Assert.That(result.Diagnostics).IsEmpty();

        var source = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "EnumJsonConverters.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

            internal static class EnumJsonConverters
            {
                public static global::System.Text.Json.JsonSerializerOptions AddGeneratedEnumJsonConverters(this global::System.Text.Json.JsonSerializerOptions options)
                {
                    options.Converters.Add(new TestNamespaceTestStatusJsonConverter());
                    return options;
                }

                private sealed class TestNamespaceTestStatusJsonConverter
                    : global::System.Text.Json.Serialization.JsonConverter<global::TestNamespace.TestStatus>
                {
                    public override global::TestNamespace.TestStatus Read(
                        ref global::System.Text.Json.Utf8JsonReader reader,
                        global::System.Type typeToConvert,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        global::TestNamespace.TestStatus enumValue;

                        if (reader.ValueTextEquals("Active"))
                        {
                            enumValue = global::TestNamespace.TestStatus.Active;
                        }
                        else if (reader.ValueTextEquals("Inactive"))
                        {
                            enumValue = global::TestNamespace.TestStatus.Inactive;
                        }
                        else if (reader.ValueTextEquals("Pending"))
                        {
                            enumValue = global::TestNamespace.TestStatus.Pending;
                        }
                        else
                        {
                            throw new global::System.Text.Json.JsonException($"Value '{reader.GetString()}' can not be serialized as '{typeof(global::TestNamespace.TestStatus).FullName}'.");
                        }

                        return enumValue;
                    }

                    public override void Write(
                        global::System.Text.Json.Utf8JsonWriter writer,
                        global::TestNamespace.TestStatus value,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        throw new global::System.NotImplementedException($"Serializing to Json is not supported for '{typeof(global::TestNamespace.TestStatus).FullName}'.");
                    }
                }
            }

            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateConverterWithMemberNames_WhenEnumHasAliasesButAliasesAreNotResolved()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            EnumJsonConverterGeneratorTestsSources.WhenEnumHasAliases);

        await Assert.That(result.Diagnostics).IsEmpty();

        var source = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "EnumJsonConverters.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

            internal static class EnumJsonConverters
            {
                public static global::System.Text.Json.JsonSerializerOptions AddGeneratedEnumJsonConverters(this global::System.Text.Json.JsonSerializerOptions options)
                {
                    options.Converters.Add(new TestNamespaceSortOrderJsonConverter());
                    return options;
                }

                private sealed class TestNamespaceSortOrderJsonConverter
                    : global::System.Text.Json.Serialization.JsonConverter<global::TestNamespace.SortOrder>
                {
                    public override global::TestNamespace.SortOrder Read(
                        ref global::System.Text.Json.Utf8JsonReader reader,
                        global::System.Type typeToConvert,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        global::TestNamespace.SortOrder enumValue;

                        if (reader.ValueTextEquals("Ascending"))
                        {
                            enumValue = global::TestNamespace.SortOrder.Ascending;
                        }
                        else if (reader.ValueTextEquals("Descending"))
                        {
                            enumValue = global::TestNamespace.SortOrder.Descending;
                        }
                        else
                        {
                            throw new global::System.Text.Json.JsonException($"Value '{reader.GetString()}' can not be serialized as '{typeof(global::TestNamespace.SortOrder).FullName}'.");
                        }

                        return enumValue;
                    }

                    public override void Write(
                        global::System.Text.Json.Utf8JsonWriter writer,
                        global::TestNamespace.SortOrder value,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        throw new global::System.NotImplementedException($"Serializing to Json is not supported for '{typeof(global::TestNamespace.SortOrder).FullName}'.");
                    }
                }
            }

            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGenerateConverterWithAliases_WhenEnumHasAliasesAndAliasesAreResolved()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            AliasAsAttribute.Source,
            EnumJsonConverterGeneratorTestsSources.WhenEnumHasAliases);

        await Assert.That(result.Diagnostics).IsEmpty();

        var source = result.Results.Single().GeneratedSources
            .First(s => s.HintName == "EnumJsonConverters.g.cs")
            .SourceText.ToString();

        var expected =
            """
            #nullable enable

            namespace DiscogsApiClient.SourceGenerator.JsonSerialization;

            internal static class EnumJsonConverters
            {
                public static global::System.Text.Json.JsonSerializerOptions AddGeneratedEnumJsonConverters(this global::System.Text.Json.JsonSerializerOptions options)
                {
                    options.Converters.Add(new TestNamespaceSortOrderJsonConverter());
                    return options;
                }

                private sealed class TestNamespaceSortOrderJsonConverter
                    : global::System.Text.Json.Serialization.JsonConverter<global::TestNamespace.SortOrder>
                {
                    public override global::TestNamespace.SortOrder Read(
                        ref global::System.Text.Json.Utf8JsonReader reader,
                        global::System.Type typeToConvert,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        global::TestNamespace.SortOrder enumValue;

                        if (reader.ValueTextEquals("asc"))
                        {
                            enumValue = global::TestNamespace.SortOrder.Ascending;
                        }
                        else if (reader.ValueTextEquals("desc"))
                        {
                            enumValue = global::TestNamespace.SortOrder.Descending;
                        }
                        else
                        {
                            throw new global::System.Text.Json.JsonException($"Value '{reader.GetString()}' can not be serialized as '{typeof(global::TestNamespace.SortOrder).FullName}'.");
                        }

                        return enumValue;
                    }

                    public override void Write(
                        global::System.Text.Json.Utf8JsonWriter writer,
                        global::TestNamespace.SortOrder value,
                        global::System.Text.Json.JsonSerializerOptions options)
                    {
                        throw new global::System.NotImplementedException($"Serializing to Json is not supported for '{typeof(global::TestNamespace.SortOrder).FullName}'.");
                    }
                }
            }

            """;

        await Assert.That(GeneratorTestHelper.NormalizeLineEndings(source))
                    .IsEqualTo(GeneratorTestHelper.NormalizeLineEndings(expected));
    }

    [Test]
    public async Task ShouldGeneratePostInitAttributes_WhenMinimalEnum()
    {
        var result = GeneratorTestHelper.RunGenerator<JsonConverterSourceGenerator>(
            EnumJsonConverterGeneratorTestsSources.WhenMinimalEnum);

        var hintNames = result.Results.Single().GeneratedSources
            .Select(s => s.HintName)
            .ToArray();

        await Assert.That(hintNames).Contains("GenerateJsonConverterAttribute.g.cs");
    }
}
