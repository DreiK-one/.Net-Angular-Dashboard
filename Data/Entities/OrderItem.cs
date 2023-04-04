namespace Data.Entities
{
    public class OrderItem : BaseEntity
    {
        public string Name { get; set; }
        public float Cost { get; set; }
        public int Quantity { get; set; }

        public virtual int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
