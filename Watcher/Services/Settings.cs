using System;
using System.IO;
using System.Text.Json;

namespace Watcher.Services;
public static class Settings
{
    public static string DestinationEmail = "";
    public static bool NotifyEmail = false;
    public static bool NotifyDesktop = false;
    public static string CaseNumber = "";
    private static string _filePath = $"{Directory.GetCurrentDirectory()}\\settings.json";
    static Settings()
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();
    }

    public static void SetCaseNumber(string caseNumber)
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        deserialized.CaseNumber = caseNumber;
        deserialized.NotifyEmail = NotifyEmail;
        deserialized.NotifyDesktop = NotifyDesktop;

        string serialized = JsonSerializer.Serialize(deserialized);

        File.WriteAllText(_filePath, serialized);
        CaseNumber = caseNumber;
    }

    public static void CreateEmptySettings()
    {
        SettingsModel emptySettings = new();

        string serialized = JsonSerializer.Serialize(emptySettings);

        File.WriteAllText(_filePath, serialized);
    }

    public static SettingsModel GetFields()
    {
        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        SettingsModel settingsModel = new()
        {
            CaseNumber = deserialized.CaseNumber,
            NotifyDesktop = deserialized.NotifyDesktop,
            NotifyEmail = deserialized.NotifyEmail,
            MailSettings = new()
            {
                MailTo = deserialized.MailSettings.MailTo
            }
        };

        return settingsModel;
    }

    public static void SetLastLink(string link)
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        deserialized.LastLink = link;
        deserialized.LastLinkDate = DateTime.Now;
        deserialized.NotifyEmail = NotifyEmail;
        deserialized.NotifyDesktop = NotifyDesktop;

        string serialized = JsonSerializer.Serialize(deserialized);

        File.WriteAllText(_filePath, serialized);
    }

    public static string GetLastLink()
    {
        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        return deserialized.LastLink;
    }

    public static bool IsLinkIsStale()
    {
        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        if (deserialized.LastLinkDate.Date < DateTime.Now.Date || deserialized.LastLink == "")
        {
            return true;
        }

        return false;
    }

    public static void SetMailSettings(EmailSettings settings)
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        deserialized.MailSettings = settings;

        string serialized = JsonSerializer.Serialize(deserialized);

        File.WriteAllText(_filePath, serialized);
    }

    public static EmailSettings GetEmailSettings()
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        EmailSettings emailSettings = new()
        {
            MailFrom = deserialized.MailSettings.MailFrom,
            MailTo = deserialized.MailSettings.MailTo,
            Password = deserialized.MailSettings.Password,
            PortNumber = deserialized.MailSettings.PortNumber,
            SmtpAddress = deserialized.MailSettings.SmtpAddress,
        };

        return emailSettings;
    }
}

public class SettingsModel
{
    public string CaseNumber { get; set; } = "";
    public bool NotifyEmail { get; set; } = false;
    public bool NotifyDesktop { get; set; } = false;
    public bool GrowToFit { get; set; } = false;
    public string LastLink { get; set; } = "";
    public DateTime LastLinkDate { get; set; } = DateTime.MinValue;
    public EmailSettings MailSettings { get; set; } = new();
}

public class EmailSettings
{
    public string MailFrom { get; set; } = "";
    public string MailTo { get; set; } = "";
    public string Password { get; set; } = "";
    public int PortNumber { get; set; } = 0;
    public string SmtpAddress { get; set; } = "";
    public bool EnableSsl { get; set; } = false;
}