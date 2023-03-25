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
using System.Diagnostics;
using Watcher.Services;

namespace Watcher.Views;
/// <summary>
/// Interaction logic for EmailConfigView.xaml
/// </summary>
public partial class EmailConfigView : Window
{
    private EmailSettings _emailSettings;

    public EmailConfigView()
    {
        InitializeComponent();
        _emailSettings = Settings.GetEmailSettings();

        FromInput.Text = _emailSettings.MailFrom;
        ToInput.Text = _emailSettings.MailTo;
        PasswordInput.Password = _emailSettings.Password;
        SMTPInput.Text = _emailSettings.SmtpAddress;
        PortInput.Text = _emailSettings.PortNumber.ToString();
        SSLCheckBox.IsChecked = _emailSettings.EnableSsl;
    }

    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://support.google.com/a/answer/176600?hl=pt-BR#gmail-smpt-option",
            UseShellExecute = true
        });
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Settings.SetMailSettings(_emailSettings);
        Close();
    }

    private void FromInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        _emailSettings.MailFrom = FromInput.Text;
    }

    private void ToInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        _emailSettings.MailTo = ToInput.Text;
    }

    private void SMTPInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        _emailSettings.SmtpAddress = SMTPInput.Text;
    }

    private void PortInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        _emailSettings.PortNumber = Convert.ToInt32(PortInput.Text);
    }
    private void PasswordInput_LostFocus(object sender, RoutedEventArgs e)
    {
        _emailSettings.Password = PasswordInput.Password;
    }

    private void SSLCheckBox_Click(object sender, RoutedEventArgs e)
    {
        _emailSettings.EnableSsl = SSLCheckBox.IsChecked ?? false;
    }
}
