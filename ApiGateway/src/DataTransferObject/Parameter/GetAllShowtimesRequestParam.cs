namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetAllShowtimesRequestParam
    {
        public DateOnly? Date { get; set; }

        public Guid? CinemaId { get; set; }

        public Guid? MovieId { get; set; }

        public string? Country { get; set; }
    }
}
