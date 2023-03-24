using System;
using System.IO;
using System.Text.Json;
using System.Windows.Markup;

namespace Watcher;
public static class Settings
{
    public static string DestinationEmail = "";
    public static bool Notify = false;
    public static string CaseNumber = "";
    private static string _filePath = $"{Directory.GetCurrentDirectory()}\\settings.json";

    public static void SetEmail(string email)
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        deserialized.DestinationEmail = email;
        deserialized.Notify = Notify;

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
        deserialized.Notify = Notify;

        string serialized = JsonSerializer.Serialize(deserialized);

        File.WriteAllText(_filePath, serialized);
        CaseNumber = caseNumber;
    }

    public static void SetNotify(bool value) => Notify = value;

    public static void CreateEmptySettings()
    {
        SettingsModel emptySettings = new()
        {
            DestinationEmail = "",
            CaseNumber = "",
            Notify = false
        };

        string emptyJson = JsonSerializer.Serialize(emptySettings);

        File.WriteAllText(_filePath, emptyJson);
    }

    public static (string, string, bool) GetFields()
    {
        if (!File.Exists(_filePath))
            CreateEmptySettings();

        string jsonString = File.ReadAllText(_filePath);
        SettingsModel deserialized = JsonSerializer.Deserialize<SettingsModel>(jsonString);

        return (deserialized.DestinationEmail, deserialized.CaseNumber, deserialized.Notify);
    }
}

class SettingsModel
{
    public string DestinationEmail { get; set; } = "";
    public string CaseNumber { get; set; } = "";
    public bool Notify { get; set; } = false;
}
