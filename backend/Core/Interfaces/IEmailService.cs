using backend.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Core.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
