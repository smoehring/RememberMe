using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using RememberMe.Ui.WPF.Services;

namespace RememberMe.Ui.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly CancellationTokenSource _cts;
    private readonly Task _monitorTask;

    public MainWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Resources.Add("services", serviceProvider);
        // We can not do a HostedService in WPF as the Window requires the main thread, so we smuggle it in here and forget about it.
        var monitor = serviceProvider.GetRequiredService<ContactMonitorService>();
        _cts = new CancellationTokenSource();
        _monitorTask = monitor.ExecuteAsync(_cts.Token);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _cts.Cancel();
    }
}