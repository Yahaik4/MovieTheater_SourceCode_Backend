using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateCustomerTypeParam : IParam
    {
        public string Name { get; set; }
        public string RoleCondition { get; set; }
    }
}
