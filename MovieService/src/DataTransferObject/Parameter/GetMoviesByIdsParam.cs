using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class GetMoviesByIdsParam : IParam
    {
        public List<Guid> MovieIds { get; set; }
    }
}
