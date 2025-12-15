using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class CreateCustomerTypeResultData : BaseResultData
    {
        public CreateCustomerTypeDataResult Data { get; set; } = null!;
    }

    public class CreateCustomerTypeDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RoleCondition { get; set; }
    }
}
