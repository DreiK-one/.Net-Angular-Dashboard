namespace Core.DTOs
{
    public class OrderDto<T> where T : class
    {
        public T? Page { get; set; }
        public double TotalPages { get; set; }
    }
}
