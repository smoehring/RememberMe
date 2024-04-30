namespace Snoval.Dev.RememberMe.Ui.Forms.Models;

public class ContactEntry
{
    public Guid Uuid { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ContactAdress { get; set; } = string.Empty;
    public DateTime LastContact { get; set; } = DateTime.Now;
}