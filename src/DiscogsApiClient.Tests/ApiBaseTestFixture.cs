using System.Threading.Tasks;
using DiscogsApiClient.Authentication.UserToken;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    public static readonly string UserAgent = "MusicLibraryManager.Tests/1.0.0";
    protected DiscogsApiClient ApiClient;
    protected IConfiguration Configuration;


    public ApiBaseTestFixture()
    {
        UserTokenAuthenticationProvider authenticationProvider = new UserTokenAuthenticationProvider();
        ApiClient = new DiscogsApiClient(authenticationProvider, UserAgent);

        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.development.json", true)
            .Build();
    }


    [OneTimeSetUp]
    public virtual async Task Initialize()
    {
        var userToken = Configuration["ApiSettings:UserToken"];
        UserTokenAuthenticationRequest authenticationRequest = new UserTokenAuthenticationRequest(userToken);
        var authenticationResponse = await ApiClient.AuthenticateAsync(authenticationRequest, default);
    }

    [OneTimeTearDown]
    public virtual void Cleanup()
    {
        ApiClient.Dispose();
    }
}