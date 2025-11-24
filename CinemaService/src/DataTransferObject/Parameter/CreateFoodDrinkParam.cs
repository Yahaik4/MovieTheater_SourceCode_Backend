namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateFoodDrinkParam
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
        public string? CreatedBy { get; set; }
    }
}
