namespace Data.Entities
{
    public class Position : BaseEntity
    {
        public string Name { get; set; }
        public float? Cost { get; set; }

        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
