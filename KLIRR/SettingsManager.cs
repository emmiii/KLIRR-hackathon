using System.IO;
using System.Text.Json;
using Model;

namespace KLIRR;

public static class SettingsManager
{
    private static readonly string SettingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KLIRR",
        "settings.json");

    public static AppSettings Settings { get; private set; } = new();

    public static void Load()
    {
        try
        {
            FileLogger.LogInfo("Loading settings...");
            if (File.Exists(SettingsFilePath))
            {
                var content = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<AppSettings>(content);
                if (settings != null)
                {
                    Settings = settings;
                    FileLogger.LogInfo("Settings loaded successfully.");
                    return;
                }
            }
            FileLogger.LogInfo("No settings file found, using defaults.");
        }
        catch (Exception ex)
        {
            FileLogger.LogError("Failed to load settings", ex);
        }
        
        Settings = new AppSettings();
    }

    public static void Save()
    {
        try
        {
            var directory = Path.GetDirectoryName(SettingsFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Settings, options);
            File.WriteAllText(SettingsFilePath, json);
            FileLogger.LogInfo("Settings saved successfully.");
        }
        catch (Exception ex)
        {
            FileLogger.LogError("Failed to save settings", ex);
        }
    }
}