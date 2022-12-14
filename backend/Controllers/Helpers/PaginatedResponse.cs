namespace backend.Controllers.Helpers
{
    public class PaginatedResponse<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedResponse(IEnumerable<T> data, int index, int length)
        {
            Data = data.Skip((index - 1) * length).Take(length).ToList();
            Total = data.Count();
        }
    }
}
