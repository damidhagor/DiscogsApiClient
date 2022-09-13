using System.Net.Http;
using System.Threading.Tasks;
using DiscogsApiClient.Authentication.UserToken;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DiscogsApiClient.Tests;

[TestFixture]
public abstract class ApiBaseTestFixture
{
    protected DiscogsApiClient ApiClient;
    protected IConfiguration Configuration;
    private readonly HttpClient _httpClient;


    public ApiBaseTestFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.Development.json", true)
            .Build();

        var userAgent = Configuration["DiscogsApiOptions:UserAgent"];

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

        var authenticationProvider = new UserTokenAuthenticationProvider();
        ApiClient = new DiscogsApiClient(_httpClient, authenticationProvider);
    }


    [OneTimeSetUp]
    public virtual async Task Initialize()
    {
        var userToken = Configuration["DiscogsApiOptions:UserToken"];
        var authenticationRequest = new UserTokenAuthenticationRequest(userToken);
        await ApiClient.AuthenticateAsync(authenticationRequest, default);
    }

    [OneTimeTearDown]
    public virtual void Cleanup()
    {
        _httpClient.Dispose();
    }
}