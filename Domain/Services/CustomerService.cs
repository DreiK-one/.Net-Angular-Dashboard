using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApiContext _context;

        public CustomerService(ApiContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id);

                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                var customers = await _context.Customers
                .OrderBy(c => c.Id)
                .ToListAsync();

                return customers;
            }
            catch (Exception)
            { 
                throw;
            }          
        }
    }
}
