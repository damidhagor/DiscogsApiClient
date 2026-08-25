using System.Globalization;

namespace DiscogsApiClient.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static long ParseIdFromLocationHeader(this HttpResponseMessage response)
    {
        var location = response.Headers.Location
            ?? throw new InvalidOperationException("The response did not contain a Location header.");

        return long.Parse(location.Segments[^1].TrimEnd('/'), CultureInfo.InvariantCulture);
    }
}
