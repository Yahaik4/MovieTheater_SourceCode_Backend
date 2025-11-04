using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetGenresParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}
