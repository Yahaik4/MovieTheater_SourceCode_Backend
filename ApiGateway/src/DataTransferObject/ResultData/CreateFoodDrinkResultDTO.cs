namespace ApiGateway.DataTransferObject.ResultData;

public class CreateFoodDrinkResultDTO : BaseResultDTO
{
    public CreateFoodDrinkDataResult Data { get; set; } = null!;
}

public class CreateFoodDrinkDataResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Size { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
}
