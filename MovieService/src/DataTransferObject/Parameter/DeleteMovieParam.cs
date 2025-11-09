using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class DeleteMovieParam : IParam
    {
        public Guid Id { get; set; }
    }
}
