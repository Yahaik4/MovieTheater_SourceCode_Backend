namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetCustomerTypesRequestParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? RoleCondition { get; set; }
    }
}
