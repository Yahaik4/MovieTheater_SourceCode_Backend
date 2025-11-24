namespace CinemaService.DataTransferObject.Parameter
{
    public class GetAllFoodDrinkParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
    }
}