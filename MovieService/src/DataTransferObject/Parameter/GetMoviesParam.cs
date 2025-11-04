using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetMoviesParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Status { get; set; }
    }
}
