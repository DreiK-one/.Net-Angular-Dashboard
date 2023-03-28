using Core.Models;


namespace Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
