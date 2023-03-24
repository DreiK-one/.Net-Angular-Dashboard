namespace backend.Domain.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
    }
}
