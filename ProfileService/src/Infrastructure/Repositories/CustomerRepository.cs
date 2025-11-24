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

        public async Task<Customer?> GetCustomerByUserId(Guid userId)
        {
            return await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task SoftDeleteByUserId(Guid userId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

            if (customer == null)
                return;

            customer.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
