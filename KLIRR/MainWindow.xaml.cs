using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Text.Json;
using System.Windows.Data;
using System.ComponentModel;
using Microsoft.Win32;
using Model;

namespace KLIRR;

public partial class MainWindow : Window
{
    public ObservableCollection<LogEntry> LogEntries { get; set; } = [];
    public ICollectionView LogEntriesView { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        LogEntriesView = CollectionViewSource.GetDefaultView(LogEntries);
        LogEntriesView.Filter = FilterLogEntries;
        DataContext = this;
        SettingsManager.Load();
        ReadData();
    }

    private bool FilterLogEntries(object obj)
    {
        if (obj is not LogEntry entry)
            return false;

        var fromDate = FilterTidFrom?.SelectedDate;
        var toDate = FilterTidTo?.SelectedDate;
        var titelFilter = FilterTitel?.Text ?? string.Empty;
        var avdelningFilter = FilterAvdelning?.Text ?? string.Empty;
        var utfallFilter = FilterUtfall?.Text ?? string.Empty;
        var taggarFilter = FilterTaggar?.Text ?? string.Empty;

        // Date range filter: include entries within the selected date range
        var matchesDateRange = (!fromDate.HasValue || entry.Tid.Date >= fromDate.Value.Date)
                            && (!toDate.HasValue || entry.Tid.Date <= toDate.Value.Date);

        return matchesDateRange
            && (string.IsNullOrEmpty(titelFilter) || entry.Titel.Contains(titelFilter, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrEmpty(avdelningFilter) || entry.Avdelning.Contains(avdelningFilter, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrEmpty(utfallFilter) || entry.Utfall.Contains(utfallFilter, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrEmpty(taggarFilter) || entry.TaggarDisplay.Contains(taggarFilter, StringComparison.OrdinalIgnoreCase));
    }

    private void Filter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        LogEntriesView?.Refresh();
    }

    private void Filter_DateChanged(object? sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        LogEntriesView?.Refresh();
    }

    private void ReadData()
    {
        var logFilePath = SettingsManager.Settings.LogFilePath;
        try
        {
            if (File.Exists(logFilePath))
            {
                var content = File.ReadAllText(logFilePath);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var data = (JsonSerializer.Deserialize<List<LogEntry>>(content) ?? []).OrderByDescending(le => le.Tid);
                    LogEntries.Clear();
                    
                    foreach (var entry in data)
                    {
                        LogEntries.Add(entry);
                    }
                    return;
                }
            }

            // File doesn't exist or is empty - initialize
            var directory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(logFilePath, "[]");
            LogEntries.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kunde inte läsa loggfilen: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Exportera_Click(object sender, RoutedEventArgs e)
    {
        if (!File.Exists(SettingsManager.Settings.LogFilePath))
        {
            MessageBox.Show("Det finns ingen loggfil att exportera.", "Exportera", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Title = "Exportera krislogg",
            Filter = "JSON-filer (*.json)|*.json|Alla filer (*.*)|*.*",
            DefaultExt = "json",
            FileName = "krislogg.json"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                File.Copy(SettingsManager.Settings.LogFilePath, saveFileDialog.FileName, overwrite: true);
                MessageBox.Show($"Loggen har exporterats till {saveFileDialog.FileName}.", "Export", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte exportera loggfilen: {ex.Message}", "Fel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void Importera_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Title = "Importera krislogg",
            Filter = "JSON-filer (*.json)|*.json|Alla filer (*.*)|*.*",
            DefaultExt = "json"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var logFilePath = SettingsManager.Settings.LogFilePath;
                var importedContent = File.ReadAllText(openFileDialog.FileName);
                var importedEntries = JsonSerializer.Deserialize<List<LogEntry>>(importedContent);

                if (importedEntries == null || importedEntries.Count == 0)
                {
                    MessageBox.Show("Filen innehåller inga loggposter.", "Importera", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingEntries = new List<LogEntry>();
                if (File.Exists(logFilePath))
                {
                    var existingContent = File.ReadAllText(logFilePath);
                    if (!string.IsNullOrWhiteSpace(existingContent))
                    {
                        existingEntries = JsonSerializer.Deserialize<List<LogEntry>>(existingContent) ?? [];
                    }
                }

                var existingSet = existingEntries
                    .Select(e => (e.Tid, e.Titel, e.Beskrivning, e.Avdelning, e.Utfall, e.TaggarDisplay, e.MachineName))
                    .ToHashSet();
                var uniqueImported = importedEntries
                    .Where(e => !existingSet.Contains((e.Tid, e.Titel, e.Beskrivning, e.Avdelning, e.Utfall, e.TaggarDisplay, e.MachineName)))
                    .ToList();

                existingEntries.AddRange(uniqueImported);

                var options = new JsonSerializerOptions { WriteIndented = true };
                var updatedJson = JsonSerializer.Serialize(existingEntries, options);
                File.WriteAllText(logFilePath, updatedJson);

                Refresh();

                MessageBox.Show($"{uniqueImported.Count} loggposter har importerats.", "Importera", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (JsonException)
            {
                MessageBox.Show("Filen kunde inte läsas. Kontrollera att det är en giltig JSON-fil.", "Importera", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void Ny_Logg_Click(object sender, RoutedEventArgs e)
    {
        LogEntryWindow logEntryWindow = new() { Owner = this };
        if (logEntryWindow.ShowDialog() == true)
        {
            Refresh();
        }
    }

    private void Loggar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Loggar.SelectedItem is LogEntry selectedEntry)
        {
            LogEntryWindow logEntryWindow = new LogEntryWindow(selectedEntry);
            logEntryWindow.Owner = this;
            logEntryWindow.ShowDialog();
        }
    }

    private void Installningar_Click(object sender, RoutedEventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow();
        settingsWindow.Owner = this;
        if (settingsWindow.ShowDialog() == true)
        {
            Refresh();
        }
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        Refresh();
    }

    private void Refresh()
    {
        ReadData();
    }
}
