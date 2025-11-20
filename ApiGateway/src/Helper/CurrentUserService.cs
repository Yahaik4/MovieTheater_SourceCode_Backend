using System.Security.Claims;

namespace ApiGateway.Helper
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public string? UserId =>
    User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
    ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
    ?? User?.FindFirst("sub")?.Value;

        public string? Email =>
            User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
            ?? User?.FindFirst(ClaimTypes.Email)?.Value
            ?? User?.FindFirst("email")?.Value;

        public string? Role =>
            User?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value
            ?? User?.FindFirst(ClaimTypes.Role)?.Value
            ?? User?.FindFirst("role")?.Value;

    }
}
