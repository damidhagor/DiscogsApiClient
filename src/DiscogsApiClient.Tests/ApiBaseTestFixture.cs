using System.Threading.Tasks;
using DiscogsApiClient.Authorization.UserToken;
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
        UserTokenAuthorizationProvider authorizationProvider = new UserTokenAuthorizationProvider();
        ApiClient = new DiscogsApiClient(authorizationProvider, UserAgent);

        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.development.json", true)
            .Build();
    }


    [OneTimeSetUp]
    public virtual async Task Initialize()
    {
        var userToken = Configuration["ApiSettings:UserToken"];
        UserTokenAuthorizationRequest authorizationRequest = new UserTokenAuthorizationRequest(userToken);
        var authorizationResponse = await ApiClient.AuthorizeAsync(authorizationRequest, default);
    }

    [OneTimeTearDown]
    public virtual void Cleanup()
    {
        ApiClient.Dispose();
    }
}