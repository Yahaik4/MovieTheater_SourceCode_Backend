using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Infrastructure.EF.Models;
using ProfileService.Infrastructure.Repositories.Interfaces;

namespace ProfileService.Infrastructure.Repositories
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
