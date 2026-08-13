using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication;

namespace DiscogsApiClientDemo.PersonalAccessToken;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;
    private readonly IDiscogsAuthenticationService _discogsAuthenticationService;

    [ObservableProperty]
    private string _userToken = "";

    [ObservableProperty]
    private string _userName = "";


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
            // Authenticate/login with your user token from your Discogs account settings.
            _discogsAuthenticationService.AuthenticateWithPersonalAccessToken(UserToken);
            var identityResponse = await _discogsApiClient.GetIdentity(cancellationToken);
            UserName = identityResponse.Username;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}
