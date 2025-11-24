using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData;

public class UpdateFoodDrinkResultData : BaseResultData
{
    public UpdateFoodDrinkDataResult Data { get; set; } = null!;
}

public class UpdateFoodDrinkDataResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Size { get; set; } = null!;
    public decimal Price { get; set; }
}