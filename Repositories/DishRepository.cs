using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Repositories
{
    public interface IDishRepository : IRepository<Dish>
    {
        Task<IEnumerable<Dish>> GetPopularDishesAsync();
    }

    public class DishRepository : Repository<Dish>, IDishRepository
    {
        public DishRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Dish>> GetPopularDishesAsync()
        {
            return await _context.Dishes
                .Where(d => d.IsPopular)
                .ToListAsync();
        }
    }
}
