using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.OAuth;

namespace DiscogsApiClientDemo.OAuth;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;
    private readonly IDiscogsOAuthAuthenticationProvider _authProvider;

    [ObservableProperty]
    public partial string AccessToken { get; set; } = "";

    [ObservableProperty]
    public partial string AccessTokenSecret { get; set; } = "";

    [ObservableProperty]
    public partial string Username { get; set; } = "";

    public MainWindow(IDiscogsApiClient discogsApiClient, IDiscogsOAuthAuthenticationProvider authProvider)
    {
        _discogsApiClient = discogsApiClient;
        _authProvider = authProvider;
        InitializeComponent();
    }

    [RelayCommand]
    public async Task Login(CancellationToken cancellationToken)
    {
        try
        {
            // Start authentication.
            var session = await _authProvider.StartAuthentication(cancellationToken);

            // Retrieve Verifier Token.
            var loginWindow = new LoginWindow(session.AuthorizeUrl, session.VerifierCallbackUrl);
            loginWindow.ShowDialog();
            var verifierToken = loginWindow.Result;

            // Complete authentication.
            (AccessToken, AccessTokenSecret) = await _authProvider.CompleteAuthentication(session, verifierToken, cancellationToken);

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
