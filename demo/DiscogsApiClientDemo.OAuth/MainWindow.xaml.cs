using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication;

namespace DiscogsApiClientDemo.OAuth;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;
    private readonly IDiscogsAuthenticationService _discogsAuthenticationService;

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
            // Start authentication.
            var session = await _discogsAuthenticationService.StartOAuthAuthentication(cancellationToken);

            // Retrieve Verifier Token.
            var loginWindow = new LoginWindow(session.AuthorizeUrl, session.VerifierCallbackUrl);
            loginWindow.ShowDialog();
            var verifierToken = loginWindow.Result;

            // Complete authentication.
            (AccessToken, AccessTokenSecret) = await _discogsAuthenticationService.CompleteOAuthAuthentication(session, verifierToken, cancellationToken);

            // If login successful (No Exceptions thrown) you can make calls to the Discogs Api.
            var identityResponse = await _discogsApiClient.GetIdentity(cancellationToken);
            Username = identityResponse.Username;
        }
        catch (Exception ex)
        {
            AccessToken = "";
            AccessTokenSecret = "";
            Username = "";
            MessageBox.Show(ex.Message, "Error");
        }
    }
}
