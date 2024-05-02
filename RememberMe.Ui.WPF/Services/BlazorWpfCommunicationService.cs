namespace Snoval.Dev.RememberMe.Ui.WPF.Services;

public class BlazorWpfCommunicationService
{
    /// <summary>
    /// Instructs the Blazor WebView to navigate to the given URL.
    /// </summary>
    public Action<string>? NavigateUrl { get; set; }

    /// <summary>
    /// Instructs the MainWindow to change the visibility state of the NotifyIcon.
    /// </summary>
    public Action<bool>? ChangeNotifyIconVisibilityState { get; set; }
}