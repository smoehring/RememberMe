using System.ComponentModel;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Snoval.Dev.RememberMe.Ui.WPF.Services;
using Application = System.Windows.Forms.Application;

namespace Snoval.Dev.RememberMe.Ui.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly CancellationTokenSource _cts;
    private readonly Task _monitorTask;
    private readonly NotifyIcon _notifyIcon;
    private readonly ConfigDataContext _config;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Resources.Add("services", serviceProvider);
        
        // We can not do a HostedService in WPF as the Window requires the main thread, so we smuggle it in here and forget about it.
        var monitor = serviceProvider.GetRequiredService<ContactMonitorService>();
        _cts = new CancellationTokenSource();
        _monitorTask = monitor.ExecuteAsync(_cts.Token);
        
        var communicationService = serviceProvider.GetRequiredService<BlazorWpfCommunicationService>();
        _config = serviceProvider.GetRequiredService<ConfigDataContext>();
        _config.Load();
        // Showing the NotifyIcon if enabled
        _notifyIcon = new NotifyIcon
        {
            Icon = RememberMe.Ui.WPF.Resources.bone,
            Visible = _config.Config.MinimizeToTray,
            Text = "RememberMe",
        };
        _notifyIcon.DoubleClick += NotifyIconOnDoubleClick;
        communicationService.ChangeNotifyIconVisibilityState += ChangeNotifyIconVisibilityState;
        
        WindowState = _config.Config.OpenMinimized ? WindowState.Minimized : WindowState.Normal;
        if (_config.Config.MinimizeToTray && WindowState == WindowState.Minimized)
        {
            Hide();
            _notifyIcon.Visible = true;
        }
    }

    private void ChangeNotifyIconVisibilityState(bool visbility)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            _notifyIcon.Visible = visbility;
        });
    }

    private void NotifyIconOnDoubleClick(object? sender, EventArgs e)
    {
        Show();
        WindowState = WindowState.Normal;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _cts.Cancel();
        base.OnClosing(e);
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized && _config.Config.MinimizeToTray)
        {
            Hide();
        }
        base.OnStateChanged(e);
    }

    public void RestoreWindow()
    {
        Show();
        WindowState = WindowState.Normal;
    }
}