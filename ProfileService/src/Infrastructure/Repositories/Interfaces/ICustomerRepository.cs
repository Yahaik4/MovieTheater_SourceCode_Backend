using ProfileService.Infrastructure.EF.Models;

namespace ProfileService.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerByUserId(Guid userId);
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task SoftDeleteByUserId(Guid userId);
    }
}
