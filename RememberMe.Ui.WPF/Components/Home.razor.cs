using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Snoval.Dev.RememberMe.Ui.WPF.Models;
using Snoval.Dev.RememberMe.Ui.WPF.Services;

namespace Snoval.Dev.RememberMe.Ui.WPF.Components;

public partial class Home : ComponentBase
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required ConfigDataContext ConfigDataContext { get; set; }
    [Inject] public required IConfiguration Configuration { get; set; }

    private ContactEntry? NextDueEntry => DisplayData.Where(entry => entry.TimespanUuid != Guid.Empty && 
                                                                     entry.LastContact.AddSeconds(ConfigDataContext.Config.Timespans.First(timespan => timespan.Uuid.Equals(entry.TimespanUuid)).Timespan) > DateTime.Now).MinBy(entry => entry.LastContact);

    private IEnumerable<ContactEntry> DisplayData => ConfigDataContext.Config.Contacts;

    protected override void OnInitialized()
    {
        var con = Configuration;
        ConfigDataContext.Load();
    }

    private void SetAsContacted(Guid uuid)
    {
        ConfigDataContext.UpdateLastContact(uuid);
        StateHasChanged();
    }
}