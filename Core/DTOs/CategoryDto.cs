using Microsoft.AspNetCore.Http;


namespace Core.DTOs
{
    public class CategoryDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? ImageSource { get; set; }
        public int? UserId { get; set; }
        public string UserAccessToken { get; set; }
        public IFormFile? file { get; set; }
    }
}
