using Microsoft.AspNetCore.Components;
using MudBlazor;
using RememberMe.Ui.WPF.Services;
using Snoval.Dev.RememberMe.Ui.Forms.Models;

namespace RememberMe.Ui.WPF.Components;

public partial class Contacts : ComponentBase
{
    private IReadOnlyList<ContactEntry> _displayData;
    private string _searchString;

    [Inject] public required ConfigDataContext ConfigDataContext { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized()
    {
        ConfigDataContext.Load();
        _displayData = ConfigDataContext.Config.Contacts;
    }
    
    private bool FilterFunc1(ContactEntry element) => FilterFunc(element, _searchString);

    private bool FilterFunc(ContactEntry element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ContactAdress.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private void RowClick(TableRowClickEventArgs<ContactEntry> obj)
    {
        NavigationManager.NavigateTo($"/Contacts/{obj.Item.Uuid}");
    }

    private void AddContact()
    {
        NavigationManager.NavigateTo($"/Contacts/{Guid.Empty}");
    }
}