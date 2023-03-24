using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Watcher.Views;

namespace Watcher.Views;
/// <summary>
/// Interaction logic for InputWindow.xaml
/// </summary>
public partial class InputWindow : Window
{
    public InputWindow()
    {
        InitializeComponent();
        if (!File.Exists($"{Directory.GetCurrentDirectory()}\\settings.json"))
            Settings.CreateEmptySettings();

        NumberInput.Text = Settings.GetFields().Item2;
        if (NumberInput.Text != "")
            StartButton.IsEnabled = true;
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        SettingsWindow settings = new();
        settings.Show();
    }

    private void NumberInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (NumberInput.Text != "")
        {
            StartButton.IsEnabled = true;
            return;
        }

        StartButton.IsEnabled = false;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new(NumberInput.Text, false);
        mainWindow.Show();
        Close();
    }
}
