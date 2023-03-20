using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;

namespace DiscogsApiClientDemo.OAuth;

public partial class LoginWindow : Window
{
    private readonly string _loginUrl;
    private readonly string _callbackUrl;

    public string Result { get; set; }

    public LoginWindow(string loginUrl, string callbackUrl)
    {
        _loginUrl = loginUrl;
        _callbackUrl = callbackUrl;
        Result = "";

        InitializeComponent();
    }

    private async void Browser_Loaded(object sender, RoutedEventArgs e)
    {
        await Browser.EnsureCoreWebView2Async();
        Browser.Source = new Uri(_loginUrl);
    }

    private void Browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (e.Uri.StartsWith(_callbackUrl))
        {
            var index = e.Uri.IndexOf("oauth_verifier");

            Result = index > -1 ? e.Uri[(index + 15)..] : "";

            Close();
        }
    }
}
