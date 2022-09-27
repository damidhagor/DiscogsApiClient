using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.UserToken;

namespace DiscogsApiClientDemo.UserToken;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;

    [ObservableProperty]
    private string _userToken = "";

    [ObservableProperty]
    private string _userName = "";


    public MainWindow(IDiscogsApiClient discogsApiClient)
    {
        // Inject the DiscogsApiClient into your class
        _discogsApiClient = discogsApiClient;

        InitializeComponent();
    }

    [RelayCommand]
    public async Task Login(CancellationToken cancellationToken)
    {
        try
        {
            // Authenticate/login with your user token from your Discogs account settings.
            var authRequest = new UserTokenAuthenticationRequest(UserToken);
            var authResponse = await _discogsApiClient.AuthenticateAsync(authRequest, cancellationToken);

            // If login successful you can make calls to the Discogs Api.
            if (authResponse.Success)
            {
                var identityResponse = await _discogsApiClient.GetIdentityAsync(cancellationToken);
                UserName = identityResponse.Username;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}
