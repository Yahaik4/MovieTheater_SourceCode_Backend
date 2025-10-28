namespace src.Helper
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }

}
