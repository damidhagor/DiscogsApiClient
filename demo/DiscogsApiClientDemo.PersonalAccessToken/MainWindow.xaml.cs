using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiscogsApiClient;
using DiscogsApiClient.Authentication.Pat;

namespace DiscogsApiClientDemo.PersonalAccessToken;

[ObservableObject]
public partial class MainWindow : Window
{
    private readonly IDiscogsApiClient _discogsApiClient;
    private readonly IDiscogsPatAuthenticationProvider _authProvider;

    [ObservableProperty]
    private string _userToken = "";

    [ObservableProperty]
    private string _userName = "";


    public MainWindow(IDiscogsApiClient discogsApiClient, IDiscogsPatAuthenticationProvider authProvider)
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
            // Authenticate/login with your user token from your Discogs account settings.
            _authProvider.Authenticate(UserToken);
            var identityResponse = await _discogsApiClient.GetIdentity(cancellationToken);
            UserName = identityResponse.Username;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }
}
