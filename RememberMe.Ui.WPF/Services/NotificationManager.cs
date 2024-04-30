using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using Snoval.Dev.RememberMe.Ui.Forms.Models;

namespace RememberMe.Ui.WPF.Services;

public class NotificationManager
{
    private readonly ILogger<NotificationManager> _logger;
    private readonly IServiceProvider _serviceProvider;

    private string ArgumentShowSummary => "showSummary";
    private string ArgumentUpdateLastContact => "updateLastContact";
    private string ArgumentShowContact => "showContact";

    public NotificationManager(ILogger<NotificationManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompatOnOnActivated;
        AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
    }
    private void ToastNotificationManagerCompatOnOnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        var args = ToastArguments.Parse(e.Argument);
        var userInput = e.UserInput;
        
        _logger.LogDebug(
            "Toast activated with args {args}, user input: {UserInput}",
            args, userInput);

        if (args.Any(pair => pair.Key.Equals(ArgumentShowSummary)))
        {
            var window = _serviceProvider.GetRequiredService<MainWindow>();
            var comms = _serviceProvider.GetRequiredService<BlazorWpfCommunicationService>();
            comms.NavigateUrl?.Invoke("/");
            Application.Current.Dispatcher.Invoke(() => window.Show());
            
        }
    }
    
    private void CurrentDomainOnProcessExit(object? sender, EventArgs e)
    {
        ToastNotificationManagerCompat.Uninstall();
    }

    public void SendSummary(List<ContactEntry> expiredContacts)
    {
        var toastContent = new ToastContentBuilder().AddText($"{expiredContacts.Count} contacts required your attention!")
            .AddText("Click here to view them")
            .AddButton("View", ToastActivationType.Foreground, ArgumentShowSummary);
        
        _logger.LogInformation("Sending Summary Notification for {count} expired contacts", expiredContacts.Count);
        toastContent.Show();
    }

    public void SendDueNotification(ContactEntry contact)
    {
        var toastContent = new ToastContentBuilder().AddText("Contact requires your attention!")
            .AddArgument(nameof(ContactEntry.Uuid), contact.Uuid.ToString("N"))
            .AddText($"{contact.Name} is due for contact")
            .AddButton("Mark as Contacted", ToastActivationType.Background, ArgumentUpdateLastContact)
            .AddButton("View", ToastActivationType.Foreground, ArgumentShowContact);
        
        _logger.LogInformation("Sending Due Notification for {contact}", contact.Uuid);
        toastContent.Show();
    }
}