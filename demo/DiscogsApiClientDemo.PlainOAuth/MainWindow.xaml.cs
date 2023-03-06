using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication;
using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClientDemo.PlainOAuth;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;
    private readonly IDiscogsAuthenticationService _discogsAuthenticationService;

    [ObservableProperty]
    private string _consumerKey = "";

    [ObservableProperty]
    private string _consumerSecret = "";

    [ObservableProperty]
    private string _accessToken = "";

    [ObservableProperty]
    private string _accessTokenSecret = "";

    [ObservableProperty]
    private string _username = "";

    public MainWindow(IDiscogsApiClient discogsApiClient, IDiscogsAuthenticationService discogsAuthenticationService)
    {
        _discogsApiClient = discogsApiClient;
        _discogsAuthenticationService = discogsAuthenticationService;
        InitializeComponent();
    }

    [RelayCommand]
    public async Task Login(CancellationToken cancellationToken)
    {
        try
        {
            // Authenticate/login with your consumer key  & secret from your Discogs application settings.
            var authRequest = new OAuthAuthenticationRequest(
                ConsumerKey,
                ConsumerSecret,
                "http://localhost/verifier_token",
                GetVerifier);

            var authResponse = await _discogsAuthenticationService.AuthenticateWithOAuth(authRequest, cancellationToken);

            // If login successful you can make calls to the Discogs Api.
            if (authResponse.Success)
            {
                var identityResponse = await _discogsApiClient.GetIdentity(cancellationToken);
                Username = identityResponse.Username;
                AccessToken = authResponse.AccessToken ?? "";
                AccessTokenSecret = authResponse.AccessTokenSecret ?? "";
            }
            else
            {
                AccessToken = "";
                AccessTokenSecret = "";
                Username = "";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private Task<string> GetVerifier(string authenticationUrl, string verifierCallbackUrl, CancellationToken cancellationToken)
    {
        var loginWindow = new LoginWindow(authenticationUrl, verifierCallbackUrl);

        loginWindow.ShowDialog();

        return Task.FromResult(loginWindow.Result);
    }
}
