using backend.Core.Models;

namespace backend.Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
