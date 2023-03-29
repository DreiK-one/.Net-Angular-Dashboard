using Core.Models;


namespace Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string email, string emailToken);
    }
}
