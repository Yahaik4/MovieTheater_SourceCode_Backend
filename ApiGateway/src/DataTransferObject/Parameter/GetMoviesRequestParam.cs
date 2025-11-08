namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetMoviesRequestParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Status { get; set; }
    }
}
