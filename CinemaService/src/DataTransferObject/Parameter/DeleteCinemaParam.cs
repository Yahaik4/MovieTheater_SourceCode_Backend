using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class DeleteCinemaParam : IParam
    {
        public Guid Id { get; set; }
    }
}
