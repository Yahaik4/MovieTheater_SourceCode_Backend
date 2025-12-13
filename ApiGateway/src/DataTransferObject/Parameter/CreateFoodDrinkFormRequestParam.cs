using Microsoft.AspNetCore.Http;

namespace ApiGateway.DataTransferObject.Parameter;

public class CreateFoodDrinkFormRequestParam
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Size { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}
