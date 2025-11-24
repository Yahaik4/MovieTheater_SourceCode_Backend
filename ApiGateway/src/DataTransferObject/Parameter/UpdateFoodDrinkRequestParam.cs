namespace ApiGateway.DataTransferObject.Parameter;

public class UpdateFoodDrinkRequestParam
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Size { get; set; }
    public decimal? Price { get; set; }
}