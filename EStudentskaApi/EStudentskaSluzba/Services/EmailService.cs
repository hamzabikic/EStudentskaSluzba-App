using System.Net;
using System.Net.Mail;

namespace EStudentskaSluzba.Services
{
    public class EmailService
    {
        public static async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Konfiguracija SMTP klijenta
                var client = new SmtpClient("smtp.gmail.com");

                    client.Port = 587; // Postavite pravilnu SMTP portu
                    client.Credentials = new NetworkCredential("adszarada55@gmail.com", "hjdq efbo ooda hygq");
                    client.EnableSsl = true;
                
                    // Kreiranje emaila
                    var message = new MailMessage();
                    message.From = new MailAddress("adszarada55@gmail.com");
                    message.To.Add(new MailAddress(toEmail));
                    message.Subject = subject;
                    message.Body = body;
               

                    // Slanje emaila
                    await client.SendMailAsync(message);

                    return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}



