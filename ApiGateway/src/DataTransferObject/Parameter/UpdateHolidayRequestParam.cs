namespace ApiGateway.DataTransferObject.Parameter
{
    public class UpdateHolidayRequestParam
    {
        public string? Name { get; set; } // Valentine’s day
        public int? Day { get; set; }
        public int? Month { get; set; }
        public decimal? ExtraPrice { get; set; } // +10k
    }
}
