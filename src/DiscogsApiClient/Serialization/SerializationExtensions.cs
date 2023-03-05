using System.Text.Json;

namespace DiscogsApiClient.Serialization;

/// <summary>
/// Contains extension methods for (de-)serializing Discogs Api messages using the <see cref="DiscogsJsonNamingPolicy"/>.
/// </summary>
internal static class SerializationExtensions
{
    public static async Task<T> DeserializeAsJsonAsync<T>(this HttpContent httpContent, CancellationToken cancellationToken)
    {
        try
        {
            var stringContent = await httpContent.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<T>(stringContent);

            return result ?? throw new SerializationDiscogsException(ExceptionMessages.GetDeserializationNoResultMessage());
        }
        catch (Exception ex)
        {
            throw new SerializationDiscogsException(ExceptionMessages.GetDeserializationFailedMessage(), ex);
        }
    }

    public static string SerializeAsJson<T>(this T payload)
    {
        try
        {
            var json = JsonSerializer.Serialize<T>(payload);

            return json ?? throw new SerializationDiscogsException(ExceptionMessages.GetSerializationNoResultMessage());
        }
        catch (Exception ex)
        {
            throw new SerializationDiscogsException(ExceptionMessages.GetSerializationFailedMessage(), ex);
        }
    }
}
