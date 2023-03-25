using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Watcher.Services;
public class MailService
{
    private readonly string _updateString;
    private EmailSettings _emailSettings;

    public MailService(string updateString)
    {
        _emailSettings = Settings.GetEmailSettings();
        _updateString = updateString;
    }

    public async Task SendEmail()
    {
        try
        {
            using MailMessage mail = new();

            mail.From = new MailAddress(_emailSettings.MailFrom);
            mail.To.Add(_emailSettings.MailTo);
            mail.Subject = "PJE Watcher - Nova Atualização";
            mail.Body = $"<h2>{_updateString}</h2> <br> <a target='_blank' href='{Settings.GetLastLink()}'>Clique aqui para acessar o PJE</a>";
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new(_emailSettings.SmtpAddress, _emailSettings.PortNumber);
            smtp.Credentials = new NetworkCredential(_emailSettings.MailFrom, _emailSettings.Password);
            smtp.EnableSsl = _emailSettings.EnableSsl;

            await smtp.SendMailAsync(mail);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
