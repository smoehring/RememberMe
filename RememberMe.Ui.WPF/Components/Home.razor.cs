using Microsoft.AspNetCore.Components;
using RememberMe.Ui.WPF.Services;
using Snoval.Dev.RememberMe.Ui.Forms.Models;

namespace RememberMe.Ui.WPF.Components;

public partial class Home : ComponentBase
{
    private IReadOnlyList<ContactEntry> _displayData;

    private ContactEntry? _nextDueEntry => _displayData.Where(entry =>
        entry.LastContact.Add(ConfigDataContext.Config.DueTimeSpan) > DateTime.Now).MinBy(entry => entry.LastContact);

    [Inject] public required ConfigDataContext ConfigDataContext { get; set; }

    protected override void OnInitialized()
    {
        ConfigDataContext.Load();
        _displayData = ConfigDataContext.Config.Contacts;
    }
}