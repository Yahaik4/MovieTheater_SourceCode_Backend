using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetCustomerTypesParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? RoleCondition { get; set; }
    }
}
