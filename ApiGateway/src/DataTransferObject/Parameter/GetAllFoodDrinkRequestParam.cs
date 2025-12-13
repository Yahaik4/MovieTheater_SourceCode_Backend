namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetAllFoodDrinkRequestParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
    }
}