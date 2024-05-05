namespace Snoval.Dev.RememberMe.Ui.WPF.Models
{
    public class ConfigModel
    {
        public int CheckInterval { get; set; } = Convert.ToInt32(TimeSpan.FromMinutes(1).TotalSeconds);
        public bool MinimizeToTray { get; set; } = false;
        public bool OpenMinimized { get; set; } = false;
        public List<ContactEntry> Contacts { get; set; } = [];

        public List<ContactTimespan> Timespans { get; set; } =
        [
            new ContactTimespan
            {
                Name = "Weekly", Timespan = Convert.ToInt32(TimeSpan.FromDays(7).TotalSeconds), Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Bi-Weekly", Timespan = Convert.ToInt32(TimeSpan.FromDays(14).TotalSeconds),
                Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Monthly", Timespan = Convert.ToInt32(TimeSpan.FromDays(30).TotalSeconds), Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Bi-Monthly", Timespan = Convert.ToInt32(TimeSpan.FromDays(60).TotalSeconds),
                Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Quarterly", Timespan = Convert.ToInt32(TimeSpan.FromDays(90).TotalSeconds),
                Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Bi-Annually", Timespan = Convert.ToInt32(TimeSpan.FromDays(180).TotalSeconds),
                Uuid = Guid.NewGuid()
            },
            new ContactTimespan()
            {
                Name = "Annually", Timespan = Convert.ToInt32(TimeSpan.FromDays(365).TotalSeconds),
                Uuid = Guid.NewGuid()
            }
        ];

    }
}
