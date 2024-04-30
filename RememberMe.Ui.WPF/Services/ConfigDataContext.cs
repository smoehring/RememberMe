using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snoval.Dev.RememberMe.Ui.Forms.Models;

namespace RememberMe.Ui.WPF.Services;

public class ConfigDataContext(ILogger<ConfigDataContext> logger)
{
    private readonly ILogger<ConfigDataContext> _logger = logger;
    private const string _appName = "snoval.dev.rememberme";
    private const string _configFileName = "config.json";
    public string AppdataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appName);
    public string ConfigFilePath => Path.Combine(AppdataFolder, _configFileName);

    public ConfigModel Config { get; set; } = new ConfigModel();

    private void CheckPrerequisite()
    {
        if (string.IsNullOrWhiteSpace(AppdataFolder) || AppdataFolder.Equals(_appName))
        {
            logger.LogCritical("AppData Folder could not be found");
            throw new Exception("AppData Folder could not be found");
        }
        if (!Directory.Exists(AppdataFolder))
        {
            logger.LogWarning("Create Appfolder at '{path}'", AppdataFolder);
            Directory.CreateDirectory(AppdataFolder);
        }

        if (!File.Exists(ConfigFilePath))
        {
            logger.LogWarning("Create Config File at '{path}'", ConfigFilePath);
            var configString = JsonConvert.SerializeObject(Config, Formatting.None);
            File.WriteAllText(ConfigFilePath, configString);
        }
    }

    /// <summary>
    /// Load the Config File
    /// </summary>
    public void Load()
    {
        CheckPrerequisite();
        logger.LogInformation("Read Config File at '{path}'", ConfigFilePath);
        var configString = File.ReadAllText(ConfigFilePath);
        Config = JsonConvert.DeserializeObject<ConfigModel>(configString);
    }

    /// Write the Config File
    public void Write()
    {
        CheckPrerequisite();
        logger.LogInformation("Write Config File at '{path}'", ConfigFilePath);
        var configString = JsonConvert.SerializeObject(Config, Formatting.None);
        File.WriteAllText(ConfigFilePath, configString);
    }

    /// <summary>
    /// Quickly sets the LastContact Date of an Contact
    /// </summary>
    /// <param name="uuid"></param>
    public void UpdateLastContact(Guid uuid)
    {
        Load();
        var contact = Config.Contacts.FirstOrDefault(c => c.Uuid == uuid);
        if (contact == null)
        {
            _logger.LogError("Trying to update LastContact of non-existing Contact Guid {guid}", uuid);
            return;
        }
        contact.LastContact = DateTime.Now;
        Write();
    }
}