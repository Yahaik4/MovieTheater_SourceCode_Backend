using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class GetUsersParam
    {
        public Guid? UserId { get; set; }
        public Guid? CinemaId { get; set; }
    }
}
