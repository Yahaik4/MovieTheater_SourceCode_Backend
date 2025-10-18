using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ProfileDbContext _context;

        public CustomerRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerById(Guid CustomerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == CustomerId);
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
