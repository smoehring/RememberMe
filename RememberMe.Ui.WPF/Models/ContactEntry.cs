namespace Snoval.Dev.RememberMe.Ui.WPF.Models;

public class ContactEntry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ContactAddress { get; set; } = string.Empty;
    public DateTime LastContact { get; set; } = DateTime.Now;
    public AddressType AddressType { get; set; }
}