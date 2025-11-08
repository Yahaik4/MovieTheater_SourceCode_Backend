using AuthenticationService.Infrastructure.EF.Models;

namespace AuthenticationService.Infrastructure.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session?> GetSessionById(Guid sessionId);
        Task<Session> CreateSession(Session session);
        Task<Session> UpdateSession(Session session);
    }
}
