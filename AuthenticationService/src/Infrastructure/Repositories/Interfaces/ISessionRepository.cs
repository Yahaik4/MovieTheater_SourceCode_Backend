using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session> CreateSession(Session session);
    }
}
