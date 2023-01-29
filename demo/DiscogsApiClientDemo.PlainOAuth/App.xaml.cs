using System.Windows;
using DiscogsApiClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscogsApiClientDemo.PlainOAuth;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                // Add DiscogsApiClient with plain OAuth authentication to the services collection
                services.AddDiscogsApiClient("AwesomeAppDemo/1.0.0");
                services.AddDiscogsPlainOAuthAuthentication();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();
        var window = _host.Services.GetRequiredService<MainWindow>();
        window.Show();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
    }
}
