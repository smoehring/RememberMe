using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using MudBlazor.Services;
using RememberMe.Ui.WPF.Services;
using Serilog;

namespace RememberMe.Ui.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var builder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
        
        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<ConfigDataContext>();
        builder.Services.AddSingleton<NotificationManager>();
        builder.Services.AddSingleton<ContactMonitorService>();
        builder.Services.AddSingleton<BlazorWpfCommunicationService>();
        builder.Services.AddWpfBlazorWebView();
        builder.Services.AddMudServices();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
        }

        _host = builder.Build();
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        var config = _host.Services.GetRequiredService<ConfigDataContext>();
        config.Load();
        if (config.Config.OpenMinimized)
        {
            if (config.Config.MinimizeToTray)
            {
                mainWindow.Hide();
            }
            else
            {
                mainWindow.WindowState = WindowState.Minimized;
                mainWindow.Show();
            }
        }
        else
        {
            mainWindow.Show();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
    }
}