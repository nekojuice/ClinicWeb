namespace ClinicWeb.SendMail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

}
