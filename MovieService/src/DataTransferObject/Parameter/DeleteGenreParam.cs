using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class DeleteGenreParam : IParam
    {
        public Guid Id { get; set; }
    }
}
