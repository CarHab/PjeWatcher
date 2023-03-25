using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Services;
public class MailService
{

    //public async Task SendEmail()
    //{
    //    try
    //    {
    //        using MailMessage mail = new();

    //        mail.From = new MailAddress(_emailFromAddress);
    //        mail.To.Add(_emailToAddress);
    //        mail.Subject = _subject;
    //        mail.Body = _body;
    //        mail.IsBodyHtml = true;

    //        using SmtpClient smtp = new(_smtpAddress, _portNumber);
    //        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
    //        smtp.EnableSsl = _enableSSL;

    //        await smtp.SendMailAsync(mail);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
}
