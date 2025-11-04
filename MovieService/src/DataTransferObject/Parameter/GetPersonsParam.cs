using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetPersonsParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}
