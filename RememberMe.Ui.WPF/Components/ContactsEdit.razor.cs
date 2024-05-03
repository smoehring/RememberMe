using Microsoft.AspNetCore.Components;
using MudBlazor;
using Snoval.Dev.RememberMe.Ui.WPF.Models;
using Snoval.Dev.RememberMe.Ui.WPF.Services;

namespace Snoval.Dev.RememberMe.Ui.WPF.Components;

public partial class ContactsEdit : ComponentBase
{
    private ContactEntry? _currentEntry;
    [Parameter] public Guid Uuid { get; set; }
    [Inject] public required ConfigDataContext ConfigDataContext { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    protected override void OnInitialized()
    {
        ConfigDataContext.Load();
        _currentEntry = Uuid.Equals(Guid.Empty) ? new ContactEntry() : ConfigDataContext.Config.Contacts.FirstOrDefault(entry => entry.Uuid.Equals(Uuid));
    }
    
    private void SaveContact()
    {
        if (_currentEntry is null) return;

        if (!ConfigDataContext.Config.Contacts.Any(entry => entry.Uuid.Equals(_currentEntry.Uuid)))
        {
            ConfigDataContext.Config.Contacts.Add(_currentEntry);
        }
        ConfigDataContext.Write();
        ConfigDataContext.Load();
        Snackbar.Add("Contact saved", Severity.Success);
        NavigationManager.NavigateTo($"/Contacts/{_currentEntry.Uuid}");
    }

    private void DeleteContact()
    {
        if (_currentEntry is null) return;

        if (ConfigDataContext.Config.Contacts.Any(entry => entry.Uuid.Equals(_currentEntry.Uuid)))
        {
            ConfigDataContext.Config.Contacts.Remove(_currentEntry);
        }
        ConfigDataContext.Write();
        NavigationManager.NavigateTo("/Contacts");
    }
    
    private void GoBack()
    {
        NavigationManager.NavigateTo("/Contacts");
    }
}