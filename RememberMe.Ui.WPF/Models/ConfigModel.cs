namespace Snoval.Dev.RememberMe.Ui.WPF.Models
{
    public class ConfigModel
    {
        public int DueTimeSpan { get; set; } = Convert.ToInt32(TimeSpan.FromDays(60).TotalSeconds);
        public int CheckInterval { get; set; } = Convert.ToInt32(TimeSpan.FromMinutes(1).TotalSeconds);
        public bool MinimizeToTray { get; set; } = false;
        public bool OpenMinimized { get; set; } = false;
        public List<ContactEntry> Contacts { get; set; } = new List<ContactEntry>();

   }
}
