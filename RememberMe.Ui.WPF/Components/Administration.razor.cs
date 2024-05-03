using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using MudBlazor;
using Snoval.Dev.RememberMe.Ui.WPF.Models;
using Snoval.Dev.RememberMe.Ui.WPF.Services;

namespace Snoval.Dev.RememberMe.Ui.WPF.Components;

public partial class Administration : ComponentBase
{
    
    [Inject] public required ILogger<Administration> Logger { get; set; }
    [Inject] public required ConfigDataContext ConfigDataContext { get; set; }
    [Inject] public required BlazorWpfCommunicationService BlazorWpfCommunicationService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required IConfiguration Configuration { get; set; }
    
    [Inject] public required NotificationManager NotificationManager { get; set; }
    
    private bool _debugExpanded;
    private EditContext _editContext;
    private ConfigModel _model;
    private string _debugUserId;

    protected override void OnInitialized()
    {
        _model = ConfigDataContext.Config;
        _editContext = new EditContext(_model);
        // _editContext.OnFieldChanged += EditContext_OnFieldChanged;
    }

    // private void EditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
    // {
    //     ConfigDataContext.Write();
    // }

    private void SendToast()
    {
        if (Guid.TryParse(_debugUserId, out var guid) && ConfigDataContext.Config.Contacts.Any(entry => entry.Uuid.Equals(guid)))
        {
            NotificationManager.SendDueNotification(ConfigDataContext.Config.Contacts.First(entry => entry.Uuid.Equals(guid)));
            return;
        }
        NotificationManager.SendSummary(ConfigDataContext.Config.Contacts.Where(c => c.LastContact.AddSeconds(ConfigDataContext.Config.DueTimeSpan) < DateTime.Now).ToList());
    }

    private void Save()
    {
        ConfigDataContext.Write();
        BlazorWpfCommunicationService.ChangeNotifyIconVisibilityState?.Invoke(_model.MinimizeToTray);
        Snackbar.Add("Settings saved", Severity.Success);
    }
}