using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerTypeRepository
    {
        Task<IEnumerable<CustomerType>> GetCustomerTypes(Guid? id, string? name, string? roleCondition);
        Task<CustomerType?> GetCustomerTypeById(Guid id);
        Task<CustomerType?> GetCustomerTypeByName(string name);
        Task<CustomerType> CreateCustomerType(CustomerType customerType);
        Task<CustomerType> UpdateCustomerType(CustomerType customerType);
    }
}
