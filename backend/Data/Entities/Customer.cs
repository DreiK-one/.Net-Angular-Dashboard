namespace backend.Data.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
    }
}
