using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class DeleteUserParam : IParam
    {
        public Guid TargetUserId { get; set; }
        public string CallerRole { get; set; } = string.Empty;
        public string? CallerPosition { get; set; }
    }
}
