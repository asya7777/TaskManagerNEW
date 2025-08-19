using System.Net;//for NetworkCredential
using System.Net.Mail;//for SmtpClient and MailMessage
using Microsoft.Extensions.Options;//for IOptions
using TaskManager.Application.Interfaces;//for IEmailSender

public class SmtpOptions//holds mail server settings we need
{
    public string Host { get; set; }//address of the SMTP server
    public int Port { get; set; }
    public string UserName { get; set; }//(oluşturacağımız gmail)
    public string Password { get; set; }//emailimin passwordü
    public bool EnableSsl { get; set; }//use encryption or not ki yollanırken okunmasın
    public string From { get; set; }
}


namespace TaskManager.Infrastructure.Services
{
    public class SmtpEmailSender:IEmailSender
    {
        private readonly SmtpOptions _opt;//_opt SMTP konfigürasyonunu tutacak,kimse karışmasın diye readonly(encapsulation)

        public SmtpEmailSender(IOptions<SmtpOptions> options)//constructor that receives the SMTP options
        {
                _opt = options.Value;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            using var client = new SmtpClient(_opt.Host, _opt.Port)//creating a new SMTP client
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_opt.UserName, _opt.Password)//set credentials
            };

            using var msg = new MailMessage(_opt.UserName, to, subject, htmlBody)//creating a new mail message
            {
                IsBodyHtml = true//for the recipient to receive HTML formatted email
            };

            Console.WriteLine($"SMTP opts: host={_opt.Host}, port={_opt.Port}, user={_opt.UserName}, ssl={_opt.EnableSsl}, pwLen={_opt.Password?.Length}");


            await client.SendMailAsync(msg);
        }
        

    }
}
