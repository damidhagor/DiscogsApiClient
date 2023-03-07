using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Authentication.PersonalAccessToken;

/// <summary>
/// An authentication provider which uses a personal access token to authenticate a user against the Discogs Api.
/// <para />
/// See <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Discogs auth flow</see>
/// </summary>
internal sealed class PersonalAccessTokenAuthenticationProvider : IPersonalAccessTokenAuthenticationProvider
{
    private string _userToken = "";

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_userToken);

    public void Authenticate(string token)
    {
        _userToken = "";

        Guard.IsNotNullOrWhiteSpace(token);

        _userToken = token;
    }

    public string CreateAuthenticationHeader()
    => string.IsNullOrWhiteSpace(_userToken)
        ? throw new UnauthenticatedDiscogsException($"The {nameof(PersonalAccessTokenAuthenticationProvider)} must be authenticated before reating an authentication header.")
        : $"Discogs token={_userToken}";

}
