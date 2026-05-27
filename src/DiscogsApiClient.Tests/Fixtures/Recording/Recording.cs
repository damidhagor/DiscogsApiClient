namespace DiscogsApiClient.Tests.Fixtures.Recording;

public sealed record Recording(
    string RequestMethod,
    string RequestUrl,
    Dictionary<string, string[]> RequestHeaders,
    string? RequestBody,
    int ResponseStatusCode,
    Dictionary<string, string[]> ResponseHeaders,
    string? ResponseBody);
