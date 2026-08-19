using DiscogsApiClient;
using DiscogsApiClient.Authentication.Pat;
using Microsoft.Extensions.DependencyInjection;

var personalAccessToken = "";

var services = new ServiceCollection()
    .AddDiscogsApiClient(options => options.UserAgent = "AwesomeAppDemo/1.0.0")
    .WithPatAuthentication()
    .BuildServiceProvider();

var authProvider = services.GetRequiredService<IDiscogsPatAuthenticationProvider>();
authProvider.Authenticate(personalAccessToken);

var discogsApiClient = services.GetRequiredService<IDiscogsApiClient>();
var identity = await discogsApiClient.GetIdentity(default);
var user = await discogsApiClient.GetUser(identity.Username, default);

Console.WriteLine($"User: {user.Username}");
Console.WriteLine($"E-Mail: {user.Email}");
