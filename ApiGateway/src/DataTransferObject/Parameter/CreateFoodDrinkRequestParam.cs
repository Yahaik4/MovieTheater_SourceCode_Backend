namespace ApiGateway.DataTransferObject.Parameter;

public class CreateFoodDrinkRequestParam
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Size { get; set; } = null!;
    public decimal Price { get; set; }
}