namespace Core.DTOs
{
    public class GetOrderDto<T> where T : class
    {
        public T? Page { get; set; }
        public double TotalPages { get; set; }
    }
}
