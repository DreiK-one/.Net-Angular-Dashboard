using Microsoft.AspNetCore.Http;


namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<string> AddFile(IFormFile file);
    }
}
