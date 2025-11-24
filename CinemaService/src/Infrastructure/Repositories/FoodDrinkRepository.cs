using CinemaService.Data;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaService.Infrastructure.Repositories.Implementations
{
    public class FoodDrinkRepository : IFoodDrinkRepository
    {
        private readonly CinemaDbContext _context;

        public FoodDrinkRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodDrink>> GetAllAsync(Guid? id, string? name, string? type, string? size)
        {
            var query = _context.FoodDrinks
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(x => x.Type == type);

            if (!string.IsNullOrWhiteSpace(size))
                query = query.Where(x => x.Size == size);

            return await query.ToListAsync();
        }

        public async Task<FoodDrink?> GetByIdAsync(Guid id)
        {
            return await _context.FoodDrinks
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<FoodDrink> CreateAsync(FoodDrink entity)
        {
            _context.FoodDrinks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FoodDrink> UpdateAsync(FoodDrink entity)
        {
            _context.FoodDrinks.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(FoodDrink entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            _context.FoodDrinks.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FoodDrink>> GetByIdsAsync(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<FoodDrink>();

            return await _context.FoodDrinks
                .Where(x => !x.IsDeleted && ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}