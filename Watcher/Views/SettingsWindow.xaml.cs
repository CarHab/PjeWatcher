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

        EmailInput.Text = fields.Item1;
        NumberInput.Text = fields.Item2;
        NotifyCheckBox.IsChecked = fields.Item3;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Settings.SetNotify(NotifyCheckBox.IsChecked ?? false);
        Settings.SetEmail(EmailInput.Text);
        Settings.SetCaseNumber(NumberInput.Text);

        Close();
    }
}
