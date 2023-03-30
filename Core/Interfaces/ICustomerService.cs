using Data.Entities;


namespace Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerById(int id);
        Task<List<Customer>> GetCustomers();           
    }
}
