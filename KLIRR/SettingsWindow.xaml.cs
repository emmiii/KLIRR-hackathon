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
}