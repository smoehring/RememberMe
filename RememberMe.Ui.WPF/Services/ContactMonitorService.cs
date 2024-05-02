using Microsoft.Extensions.Logging;

namespace Snoval.Dev.RememberMe.Ui.WPF.Services;

public class ContactMonitorService(ILogger<ContactMonitorService> logger, NotificationManager notificationManager, ConfigDataContext configDataContext)
{
    private PeriodicTimer? _timer;
    private List<Guid> _contactsNotified = new();

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(configDataContext.Config.CheckInterval));
        configDataContext.Load();
        // Sending summary notification of all expired contacts
        var expiredContacts = configDataContext.Config.Contacts.Where(c => c.LastContact.AddSeconds(configDataContext.Config.DueTimeSpan) < DateTime.Now).ToList();
        if (expiredContacts.Count != 0)
        {
            notificationManager.SendSummary(expiredContacts); ;
            logger.LogDebug("Adding {count} contacts to the list of notified contacts", expiredContacts.Count);
            _contactsNotified.AddRange(expiredContacts.Select(c => c.Uuid));
            
        }
        
        do
        {
            // Check if previous expired Contracts are now resolved
            var resolvedContacts = configDataContext.Config.Contacts.Where(entry => _contactsNotified.Contains(entry.Uuid) && entry.LastContact.AddSeconds(configDataContext.Config.DueTimeSpan) > DateTime.Now).ToList();
            if (resolvedContacts.Count != 0)
            {
                logger.LogDebug("Removing {count} contacts from the list of notified contacts", resolvedContacts.Count);
                _contactsNotified.RemoveAll(c => resolvedContacts.Select(c => c.Uuid).Contains(c));
            }
            
            // Check for new expired contacts
            var newExpiredContacts = configDataContext.Config.Contacts.Where(c => !_contactsNotified.Contains(c.Uuid) && c.LastContact.AddSeconds(configDataContext.Config.DueTimeSpan) < DateTime.Now).ToList();
            if (newExpiredContacts.Count != 0)
            {
                foreach (var contact in newExpiredContacts)
                {
                    notificationManager.SendDueNotification(contact);
                    logger.LogDebug("Adding {contact} to the list of notified contacts", contact.Uuid);
                    _contactsNotified.Add(contact.Uuid);
                }
            }
            
            
        } while (await _timer.WaitForNextTickAsync(stoppingToken));
    }
}