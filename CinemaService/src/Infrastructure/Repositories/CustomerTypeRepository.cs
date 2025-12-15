using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly CinemaDbContext _context;

        public CustomerTypeRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerType> CreateCustomerType(CustomerType customerType)
        {
            await _context.CustomerTypes.AddAsync(customerType);
            await _context.SaveChangesAsync();
            return customerType;
        }

        public async Task<CustomerType?> GetCustomerTypeById(Guid id)
        {
            return await _context.CustomerTypes.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<CustomerType?> GetCustomerTypeByName(string name)
        {
            return await _context.CustomerTypes.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<CustomerType>> GetCustomerTypes(Guid? id, string? name, string? roleCondition)
        {
            var query = _context.CustomerTypes.Where(c => c.IsDeleted != true)
                                              .AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(c => c.Id == id.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(roleCondition))
            {
                query = query.Where(c => c.RoleCondition.Contains(roleCondition));
            }

            return await query.ToListAsync();
        }

        public async Task<CustomerType> UpdateCustomerType(CustomerType customerType)
        {
            _context.CustomerTypes.Update(customerType);
            await _context.SaveChangesAsync();
            return customerType;
        }
    }
}
