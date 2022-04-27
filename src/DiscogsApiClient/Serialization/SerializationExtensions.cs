using System.Text.Json;
using System.Text.Json.Serialization;
using DiscogsApiClient.Exceptions;

namespace DiscogsApiClient.Serialization;

/// <summary>
/// Contains extension methods for (de-)serializing Discogs Api messages using the <see cref="DiscogsJsonNamingPolicy"/>.
/// </summary>
internal static class SerializationExtensions
{
    private static readonly DiscogsJsonNamingPolicy _jsonNamingPolicy = new();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = _jsonNamingPolicy,
        Converters = { new JsonStringEnumConverter(_jsonNamingPolicy) }
    };

    public static async Task<T> DeserializeAsJsonAsync<T>(this HttpContent httpContent, CancellationToken cancellationToken)
    {
        try
        {
            var stringContent = await httpContent.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(stringContent, _jsonSerializerOptions);

            return result ?? throw new SerializationDiscogsException("Deserialization returned no result.");
        }
        catch (Exception ex)
        {
            throw new SerializationDiscogsException("Deserialization failed.", ex);
        }
    }

    public static string SerializeAsJson<T>(this T payload)
    {
        try
        {
            var json = JsonSerializer.Serialize<T>(payload, _jsonSerializerOptions);

            return json ?? throw new SerializationDiscogsException("Serialization returned no result.");
        }
        catch (Exception ex)
        {
            throw new SerializationDiscogsException("Serialization failed.", ex);
        }
    }
}