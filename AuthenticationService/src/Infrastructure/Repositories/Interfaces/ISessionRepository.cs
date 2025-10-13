using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session?> GetSessionById(Guid sessionId);
        Task<Session> CreateSession(Session session);
        Task<Session> UpdateSession(Session session);
    }
}
