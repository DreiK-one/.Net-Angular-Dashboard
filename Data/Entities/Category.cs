namespace Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageSource { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IEnumerable<Position> Positions { get; set; }
    }
}
