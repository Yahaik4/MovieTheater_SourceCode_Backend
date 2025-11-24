using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.Infrastructure.Repositories.Interfaces
{
    public interface IFoodDrinkRepository
    {
        Task<List<FoodDrink>> GetAllAsync(Guid? id, string? name, string? type, string? size);
        Task<FoodDrink?> GetByIdAsync(Guid id);
        Task<FoodDrink> CreateAsync(FoodDrink entity);
        Task<FoodDrink> UpdateAsync(FoodDrink entity);
        Task DeleteAsync(FoodDrink entity);
        Task<List<FoodDrink>> GetByIdsAsync(List<Guid> ids);
    }
}