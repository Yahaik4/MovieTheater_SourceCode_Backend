using ProfileService.Infrastructure.EF.Models;

namespace ProfileService.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerById(Guid CustomerId);
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
    }
}
