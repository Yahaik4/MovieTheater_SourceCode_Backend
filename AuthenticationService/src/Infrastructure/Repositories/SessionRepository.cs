using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly AuthDbContext _context;

        public SessionRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Session?> GetSessionById(Guid sessionId)
        {
            return await _context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);
        }

        public async Task<Session> CreateSession(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<Session> UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }
    }
}
