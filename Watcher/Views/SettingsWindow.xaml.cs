using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Watcher.Services;

namespace Watcher.Views;
/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        var fields = Settings.GetFields();

        EmailInput.Text = fields.DestinationEmail;
        NumberInput.Text = fields.CaseNumber;
        EmailNotifyCheckBox.IsChecked = fields.NotifyEmail;
        DesktopNotifyCheckBox.IsChecked = fields.NotifyDesktop;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Settings.NotifyEmail = EmailNotifyCheckBox.IsChecked ?? false;
        Settings.NotifyDesktop = DesktopNotifyCheckBox.IsChecked ?? false;
        Settings.SetEmail(EmailInput.Text);
        Settings.SetCaseNumber(NumberInput.Text);

        Close();
    }
}
