using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class UpdateGenreParam : IParam
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
    }
}
