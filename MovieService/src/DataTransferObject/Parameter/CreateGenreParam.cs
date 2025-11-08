using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class CreateGenreParam : IParam
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
}
