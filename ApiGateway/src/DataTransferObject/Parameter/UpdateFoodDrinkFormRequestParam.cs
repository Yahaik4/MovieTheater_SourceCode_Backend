using Microsoft.AspNetCore.Http;

namespace ApiGateway.DataTransferObject.Parameter;

public class UpdateFoodDrinkFormRequestParam
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Size { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}
