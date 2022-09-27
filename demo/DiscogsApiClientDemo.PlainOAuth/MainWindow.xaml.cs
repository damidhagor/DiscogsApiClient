using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.PlainOAuth;

namespace DiscogsApiClientDemo.PlainOAuth;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;

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

    public MainWindow(IDiscogsApiClient discogsApiClient)
    {
        _discogsApiClient = discogsApiClient;

        InitializeComponent();
    }

    [RelayCommand]
    public async Task Login(CancellationToken cancellationToken)
    {
        try
        {
            // Authenticate/login with your consumer key  & secret from your Discogs application settings.
            var authRequest = new PlainOAuthAuthenticationRequest(
                "AwesomeAppDemo/1.0.0",
                ConsumerKey,
                ConsumerSecret,
                "http://localhost/verifier_token",
                GetVerifier);

            var authResponse = await _discogsApiClient.AuthenticateAsync(authRequest, cancellationToken);

            // If login successful you can make calls to the Discogs Api.
            if (authResponse is PlainOAuthAuthenticationResponse oauthResponse
                && authResponse.Success)
            {
                var identityResponse = await _discogsApiClient.GetIdentityAsync(cancellationToken);
                Username = identityResponse.Username;
                AccessToken = oauthResponse.AccessToken ?? "";
                AccessTokenSecret = oauthResponse.AccessSecret ?? "";
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
S