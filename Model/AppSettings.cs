using System.Text.Json;

namespace Model;

public class AppSettings
{
    public const string DefaultLogFilePath = @"C:\krislogg.json";
    
    public string LogFilePath { get; set; } = DefaultLogFilePath;
}