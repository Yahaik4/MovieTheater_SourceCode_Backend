using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetAllFoodDrinkResultData : BaseResultData
    {
        public List<GetAllFoodDrinkDataResult> Data { get; set; }
            = new();
    }

    public class GetAllFoodDrinkDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
    }
}
