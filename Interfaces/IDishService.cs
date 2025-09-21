using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IDishService
    {
        Task<IEnumerable<DishResponse>> GetAllDishesAsync();
        Task<DishResponse> GetDishByIdAsync(int id);
        Task<DishResponse> CreateDishAsync(CreateDishRequest request);
        Task<DishResponse> UpdateDishAsync(int id, UpdateDishRequest request);
        Task<bool> DeleteDishAsync(int id);
        Task<IEnumerable<DishResponse>> GetPopularDishesAsync();
    }
}
