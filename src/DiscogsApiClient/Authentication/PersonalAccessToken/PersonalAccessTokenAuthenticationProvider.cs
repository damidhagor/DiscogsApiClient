using DiscogsApiClient.Authentication.PersonalAccessToken;

namespace DiscogsApiClient.Authentication.UserToken;

/// <summary>
/// An authentication provider using a user's personal access token.
/// <para />
/// See <see href="https://www.discogs.com/developers#page:authentication,header:authentication-discogs-auth-flow">Discogs auth flow</see>
/// </summary>
internal sealed class PersonalAccessTokenAuthenticationProvider : IPersonalAccessTokenAuthenticationProvider
{
    private string _userToken = "";

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_userToken);

    public void Authenticate(string token)
    {
        Guard.IsNotNullOrWhiteSpace(token);

        _userToken = token;
    }

    public string CreateAuthenticationHeader() => $"Discogs token={_userToken}";
}
