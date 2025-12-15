namespace ApiGateway.DataTransferObject.ResultData
{
    public class UpdateCustomerTypeResultDTO : BaseResultDTO
    {
        public UpdateCustomerTypeDataResult Data { get; set; }
    }

    public class UpdateCustomerTypeDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RoleCondition { get; set; }
    }
}
