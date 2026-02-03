using Model;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace KLIRR;

public partial class LogEntryWindow : Window
{
    private readonly bool _isReadOnly;

    public LogEntryWindow()
    {
        InitializeComponent();
        _isReadOnly = false;
    }

    public LogEntryWindow(LogEntry entry)
    {
        InitializeComponent();
        _isReadOnly = true;
        
        TitleTextBox.Text = entry.Titel;
        DepartmentTextBox.Text = entry.Avdelning;
        MessageTextBox.Text = entry.Beskrivning;
        DecisionTextBox.Text = entry.Utfall;
        TagTextBox.Text = $"#{string.Join(" #", entry.Taggar)}";

        // Read only mode
        if (FindName("Spara") is Button sparaButton)
        {
            sparaButton.Visibility = Visibility.Collapsed;
        }
        if (FindName("Avbryt") is Button avbrytButton)
        {
            avbrytButton.Content = "OK";
        }
        TitleTextBox.IsReadOnly = true;
        DepartmentTextBox.IsReadOnly = true;
        MessageTextBox.IsReadOnly = true;
        DecisionTextBox.IsReadOnly = true;
        TagTextBox.IsReadOnly = true;

        Title = $"Loggpost: {entry.Titel}";
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_isReadOnly)
        {
            Close();
            return;
        }

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
        { 
            errors.Add("Titel");
        }
        if (string.IsNullOrWhiteSpace(DepartmentTextBox.Text))
        {
            errors.Add("Avdelning");
        }
        if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
        { 
            errors.Add("Beskrivning");
        }

        if (errors.Count > 0)
        {
            MessageBox.Show($"Följande fält måste anges: {string.Join(", ", errors)}.", "Valideringsfel", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newEntry = new LogEntry
        {
            Tid = DateTime.Now,
            Titel = TitleTextBox.Text.Trim(),
            Beskrivning = MessageTextBox.Text.Trim(),
            Avdelning = DepartmentTextBox.Text.Trim(),
            Utfall = DecisionTextBox.Text.Trim(),
            Taggar = [.. TagTextBox.Text.Split([',', ';'], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim())]
        };
        await SaveEntryAsync(newEntry);
        DialogResult = true;
        Close();
    }

    private static async Task SaveEntryAsync(LogEntry entry)
    {
        var filePath = SettingsManager.Settings.LogFilePath;
        var entries = new List<LogEntry>();

        if (File.Exists(filePath))
        {
            var json = await File.ReadAllTextAsync(filePath);
            if (!string.IsNullOrWhiteSpace(json))
            {
                entries = JsonSerializer.Deserialize<List<LogEntry>>(json)?? new List<LogEntry>();
            }
        }
        entries.Add(entry);

        var options = new JsonSerializerOptions { WriteIndented = true };
        var updatedJson = JsonSerializer.Serialize(entries, options);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}