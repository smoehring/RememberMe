using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Snoval.Dev.RememberMe.Ui.WPF.Models;

namespace Snoval.Dev.RememberMe.Ui.WPF.Services;

public class ContactMonitorService(ILogger<ContactMonitorService> logger, NotificationManager notificationManager, ConfigDataContext configDataContext)
{
    private PeriodicTimer? _timer;
    private List<Guid> _contactsNotified = new();
    private List<ContactEntry> _delayedNotifications = new();
    private const int IdleTimeMinutes = 5;

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(configDataContext.Config.CheckInterval));
        configDataContext.Load();
        // Sending summary notification of all expired contacts
        var expiredContacts = configDataContext.Config.Contacts.Where(c => c.TimespanUuid != Guid.Empty 
                                                                           && c.LastContact.AddSeconds(configDataContext.Config.Timespans.First(timespan => timespan.Uuid.Equals(c.TimespanUuid)).Timespan) < DateTime.Now).ToList();
        if (expiredContacts.Count != 0)
        {
            notificationManager.SendSummary(expiredContacts); ;
            logger.LogDebug("Adding {count} contacts to the list of notified contacts", expiredContacts.Count);
            _contactsNotified.AddRange(expiredContacts.Select(c => c.Uuid));
            
        }
        
        do
        {
            // Check if previous expired Contracts are now resolved
            var resolvedContacts = configDataContext.Config.Contacts.Where(entry => _contactsNotified.Contains(entry.Uuid) 
                && entry.TimespanUuid != Guid.Empty 
                && entry.LastContact.AddSeconds(configDataContext.Config.Timespans.First(timespan => timespan.Uuid.Equals(entry.TimespanUuid)).Timespan) > DateTime.Now).ToList();
            if (resolvedContacts.Count != 0)
            {
                logger.LogDebug("Removing {count} contacts from the list of notified contacts", resolvedContacts.Count);
                _contactsNotified.RemoveAll(c => resolvedContacts.Select(c => c.Uuid).Contains(c));
            }
            
            // Check for new expired contacts
            var newExpiredContacts = configDataContext.Config.Contacts.Where(c => !_contactsNotified.Contains(c.Uuid) 
                && c.TimespanUuid != Guid.Empty 
                && c.LastContact.AddSeconds(configDataContext.Config.Timespans.First(timespan => timespan.Uuid.Equals(c.TimespanUuid)).Timespan) < DateTime.Now).ToList();

            if (_delayedNotifications.Count > 0)
            {
                newExpiredContacts.AddRange(_delayedNotifications);
            }
            
            if (newExpiredContacts.Count != 0)
            {
                foreach (var contact in newExpiredContacts)
                {
                    // Check if User is even around, otherwise store the notification for later
                    var idleTime = TimeSpan.FromSeconds(GetLastInputTime());
                    
                    if (idleTime > TimeSpan.FromMinutes(IdleTimeMinutes))
                    {
                        if (!_delayedNotifications.Contains(contact))
                        {
                            logger.LogDebug("User is not around, storing notification for later");
                            _delayedNotifications.Add(contact);
                        }
                        continue;
                    }

                    _contactsNotified.Add(contact.Uuid);
                    notificationManager.SendDueNotification(contact);
                    if (_delayedNotifications.Contains(contact))
                    {
                        _delayedNotifications.RemoveAll(entry => entry.Uuid == contact.Uuid);
                    }
                    logger.LogDebug("Adding {contact} to the list of notified contacts", contact.Uuid);

                }
            }
            
            
        } while (await _timer.WaitForNextTickAsync(stoppingToken));
    }

    #region GlobalUserActivity

    [DllImport("user32.dll")]
    static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [StructLayout(LayoutKind.Sequential)]
    private struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
        [MarshalAs(UnmanagedType.U4)] public int cbSize;
        [MarshalAs(UnmanagedType.U4)] public uint dwTime;
    }

    static uint GetLastInputTime()
    {
        uint idleTime = 0;
        LASTINPUTINFO lastInputInfo = new();
        lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
        lastInputInfo.dwTime = 0;
        
        var envTicks = (uint) Environment.TickCount;
        if (GetLastInputInfo(ref lastInputInfo))
        {
            var lastInputTick = lastInputInfo.dwTime;
            idleTime = envTicks - lastInputTick;
        }
        return ((idleTime > 0) ? (idleTime / 1000) : 0);
    }

    #endregion
    
}