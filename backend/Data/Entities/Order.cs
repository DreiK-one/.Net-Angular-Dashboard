using System.ComponentModel.DataAnnotations.Schema;


namespace backend.Data.Entities
{
    public class Order : BaseEntity
    {
        public Customer Customer { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public DateTime Placed { get; set; }
        public DateTime? Completed { get; set; }
    }
}
