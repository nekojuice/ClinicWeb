using MimeKit;
//using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Net.Smtp;


//製作信件內容介面
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:MailServer"], int.Parse(_configuration["EmailSettings:MailPort"]), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

        }
        catch (Exception)
        {

            throw new Exception("郵件發送失敗");
        }
       
    }

 

}



