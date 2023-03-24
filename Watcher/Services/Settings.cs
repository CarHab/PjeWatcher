using System;
using System.IO;
using System.Net.NetworkInformation;
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

    public static void SetEmail(string email)
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        deserialized.DestinationEmail = email;
        deserialized.NotifyEmail = NotifyEmail;
        deserialized.NotifyDesktop = NotifyDesktop;

        string serialized = JsonSerializer.Serialize(deserialized);

        File.WriteAllText(_filePath, serialized);
        DestinationEmail = email;

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
        SettingsModel emptySettings = new()
        {
            DestinationEmail = "",
            CaseNumber = "",
            NotifyEmail = false,
            NotifyDesktop = false,
            GrowToFit = false,
            LastLink = "",
            LastLinkDate = DateTime.MinValue
        };

        string serialized = JsonSerializer.Serialize(emptySettings);

        File.WriteAllText(_filePath, serialized);
    }

    public static SettingsModel GetFields()
    {
        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        SettingsModel settingsModel = new()
        {
            DestinationEmail = deserialized.DestinationEmail,
            CaseNumber = deserialized.CaseNumber,
            NotifyDesktop = deserialized.NotifyDesktop,
            NotifyEmail = deserialized.NotifyEmail,
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
}

public class SettingsModel
{
    public string DestinationEmail { get; set; } = "";
    public string CaseNumber { get; set; } = "";
    public bool NotifyEmail { get; set; } = false;
    public bool NotifyDesktop { get; set; }
    public bool GrowToFit { get; set; }
    public string LastLink { get; set; } = "";
    public DateTime LastLinkDate { get; set; }
}
