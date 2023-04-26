namespace Core.DTOs
{
    public class OrderDto
    {
        public int CustomerId{ get; set; }
        public List<OrderItemDto> OrderItemsDtos { get; set; }

    }
}
