using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snoval.Dev.RememberMe.Ui.Forms.Models
{
    public class ConfigModel
    {
        public TimeSpan DueTimeSpan { get; set; } = TimeSpan.FromDays(60);
        public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(1);
        public bool MinimizeToTray { get; set; } = false;
        public bool OpenMinimized { get; set; } = false;
        public List<ContactEntry> Contacts { get; set; } = new List<ContactEntry>();

   }
}
