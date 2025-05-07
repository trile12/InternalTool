using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Uno.Extensions;
using Uno.Extensions.Hosting;
using Uno.Extensions.Navigation;
using InternalTool.ViewModels;
using Microsoft.JSInterop;

namespace InternalTool;

public sealed partial class App : Application
{
    private IHost Host { get; set; }

    public App()
    {
        this.InitializeComponent();
    }
    public IHost GetHost()
    {
        return Host;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
                .UseLogging()
                .UseConfiguration()
                .UseSerialization()
                .UseLocalization()
                .UseNavigation()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<SyncOrderViewModel>();
                })
            );

        Host = builder.Build();

        var window = builder.Window;
        var frame = new Frame();
        window.Content = frame;
        frame.Navigate(typeof(MainPage));
        window.Activate();
    }
}
