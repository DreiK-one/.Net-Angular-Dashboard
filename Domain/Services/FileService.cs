using Core.Interfaces;
using Microsoft.AspNetCore.Http;


namespace Domain.Services
{
    public class FileService : IFileService
    {
        private readonly int _limit = 1024 * 1024 * 5;

        public async Task<string> AddFile(IFormFile file)
        {
            if (FileFilter(file) == false)
            {
                throw new NotImplementedException();
            }

            var date = DateTime.Now.ToString("ddmmyyyy-HHmmss_FFF");
            var fileName = $"{date}-{file.FileName}";
            var path = "D:\\" + fileName;

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return path;
        }

        private bool FileFilter(IFormFile file)
        {
            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                return true;
            }

            return false;
        }
    }
}
