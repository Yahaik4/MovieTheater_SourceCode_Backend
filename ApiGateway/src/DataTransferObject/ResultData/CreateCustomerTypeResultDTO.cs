namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreateCustomerTypeResultDTO : BaseResultDTO
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
