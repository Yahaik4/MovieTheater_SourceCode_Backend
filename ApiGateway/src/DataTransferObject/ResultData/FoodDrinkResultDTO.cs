namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetAllFoodDrinkResultDTO : BaseResultDTO
    {
        public List<GetAllFoodDrinkDataResult> Data { get; set; } = new();
    }

    public class GetAllFoodDrinkDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
    }
}