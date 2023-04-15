namespace Core.DTOs
{
    public class PositionDto
    {
        public string Name { get; set; }
        public float? Cost { get; set; }
        public int CategoryId { get; set; }
        public int? CreatedByUserId { get; set; }
        public string UserAccessToken { get; set; }
    }
}
