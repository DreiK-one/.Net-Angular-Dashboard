namespace Data.Entities
{
    public class Position : BaseEntity
    {
        public string Name { get; set; }
        public float? Cost { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
