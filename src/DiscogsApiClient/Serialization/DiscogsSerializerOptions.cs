using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscogsApiClient.Serialization;

internal static class DiscogsSerializerOptions
{
    public static readonly JsonSerializerOptions Options;

    static DiscogsSerializerOptions()
    {
        var jsonNamingPolicy = new DiscogsJsonNamingPolicy();
        Options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = jsonNamingPolicy,
            Converters = { new JsonStringEnumConverter(jsonNamingPolicy) }
        };
    }
}
