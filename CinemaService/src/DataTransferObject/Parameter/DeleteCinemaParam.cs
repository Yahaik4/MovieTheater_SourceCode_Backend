using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class DeleteCinemaParam : IParam
    {
        public Guid Id { get; set; }
    }
}
