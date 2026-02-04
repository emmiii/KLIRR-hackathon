using System.Diagnostics;
using System.IO;
using System.Windows;

namespace KLIRR;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        LogFilePathTextBox.Text = SettingsManager.Settings.LogFilePath;
    }

    private void Spara_Sokvag_Click(object sender, RoutedEventArgs e)
    {
        SettingsManager.Settings.LogFilePath = LogFilePathTextBox.Text;
        SettingsManager.Save();
        Close();
    }

    private void Stang_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OppnaApplikationslogg_Click(object sender, RoutedEventArgs e)
    {
        var logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "KLIRR",
            "app.log");

        if (File.Exists(logFilePath))
        {
            Process.Start("explorer.exe", $"/select,\"{logFilePath}\"");
        }
        else
        {
            var folderPath = Path.GetDirectoryName(logFilePath);
            if (Directory.Exists(folderPath))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("Loggmappen finns inte ännu.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}