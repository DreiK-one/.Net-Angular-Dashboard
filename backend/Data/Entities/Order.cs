using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public DateTime Placed { get; set; }
        public DateTime? Completed { get; set; }
    }
}
