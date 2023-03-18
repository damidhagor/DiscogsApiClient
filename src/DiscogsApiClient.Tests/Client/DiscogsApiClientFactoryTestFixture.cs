namespace DiscogsApiClient.Tests.Client;

[TestFixture]
public sealed class DiscogsApiClientFactoryTestFixture : ApiBaseTestFixture
{
    [Test]
    public void Standalone_Client_Can_Be_Created()
    {
        var options = new DiscogsApiClientOptions
        {
            BaseUrl = Configuration["DiscogsApiOptions:BaseUrl"]!,
            UserAgent = Configuration["DiscogsApiOptions:UserAgent"]!,
            UseRateLimiting = true
        };

        DiscogsApiClient.Create(options);
    }

    [Test]
    public async Task Standalone_Client_Can_Be_Authenticated()
    {
        var userToken = Configuration["DiscogsApiOptions:UserToken"]!;
        var options = new DiscogsApiClientOptions
        {
            BaseUrl = Configuration["DiscogsApiOptions:BaseUrl"]!,
            UserAgent = Configuration["DiscogsApiOptions:UserAgent"]!,
            UseRateLimiting = true
        };

        var (discogsApiClient, authenticationService) = DiscogsApiClient.Create(options);

        authenticationService.AuthenticateWithPersonalAccessToken(userToken);

        Assert.IsTrue(authenticationService.IsAuthenticated);
        var identity =await discogsApiClient.GetIdentity(default);
        Assert.IsNotNull(identity);
        Assert.AreEqual("DamIDhagor", identity.Username);
    }
}
