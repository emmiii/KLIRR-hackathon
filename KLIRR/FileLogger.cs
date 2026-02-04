using System.IO;

namespace KLIRR;

public static class FileLogger
{
    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "KLIRR",
        "app.log");

    private static readonly object _lock = new();

    public static void LogInfo(string message)
    {
        Log("INFO", message);
    }

    public static void LogError(string message, Exception? exception = null)
    {
        var fullMessage = exception != null 
            ? $"{message} - {exception.GetType().Name}: {exception.Message}" 
            : message;
        Log("ERROR", fullMessage);
    }

    private static void Log(string level, string message)
    {
        try
        {
            var directory = Path.GetDirectoryName(LogFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message} [Machine: {Environment.MachineName}]";
            
            lock (_lock)
            {
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
        }
        catch
        {
            // Silently fail if logging fails
        }
    }
}