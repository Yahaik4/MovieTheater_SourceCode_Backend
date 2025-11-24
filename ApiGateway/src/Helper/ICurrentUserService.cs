namespace ApiGateway.Helper
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? Role { get; }

        string? Position { get; }
        string? CinemaId { get; }
    }

}
