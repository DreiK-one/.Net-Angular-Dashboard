using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Entities
{
    public class Order : BaseEntity
    {
        [Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public DateTime Placed { get; set; }
        public DateTime? Completed { get; set; }

        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
