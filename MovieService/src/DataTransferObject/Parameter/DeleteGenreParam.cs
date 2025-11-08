using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class DeleteGenreParam : IParam
    {
        public Guid Id { get; set; }
    }
}
