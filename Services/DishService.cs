using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Models;
using RestaurantBookingSystem.Repositories;

namespace RestaurantBookingSystem.Services
{
    public class DishService : IDishService
    {
        private readonly IRepository<Dish> _dishRepository;

        public DishService(IRepository<Dish> dishRepository)
        {
            _dishRepository = dishRepository;
        }

        public async Task<IEnumerable<DishResponse>> GetAllDishesAsync()
        {
            var dishes = await _dishRepository.GetAllAsync();
            return dishes.Select(d => new DishResponse
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                IsPopular = d.IsPopular,
                ImageUrl = d.ImageUrl
            });
        }

        public async Task<DishResponse> GetDishByIdAsync(int id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null) return null;

            return new DishResponse
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                IsPopular = dish.IsPopular,
                ImageUrl = dish.ImageUrl
            };
        }

        public async Task<DishResponse> CreateDishAsync(CreateDishRequest request)
        {
            var dish = new Dish
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                IsPopular = request.IsPopular,
                ImageUrl = request.ImageUrl
            };

            await _dishRepository.AddAsync(dish);
            await _dishRepository.SaveChangesAsync();

            return new DishResponse
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                IsPopular = dish.IsPopular,
                ImageUrl = dish.ImageUrl
            };
        }

        public async Task<DishResponse> UpdateDishAsync(int id, UpdateDishRequest request)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null) return null;

            dish.Name = request.Name;
            dish.Price = request.Price;
            dish.Description = request.Description;
            dish.IsPopular = request.IsPopular;
            dish.ImageUrl = request.ImageUrl;

            _dishRepository.Update(dish);
            await _dishRepository.SaveChangesAsync();

            return new DishResponse
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Description = dish.Description,
                IsPopular = dish.IsPopular,
                ImageUrl = dish.ImageUrl
            };
        }

        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _dishRepository.GetByIdAsync(id);
            if (dish == null) return false;

            _dishRepository.Delete(dish);
            return await _dishRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DishResponse>> GetPopularDishesAsync()
        {
            var popularDishes = await ((IDishRepository)_dishRepository).GetPopularDishesAsync();
            return popularDishes.Select(d => new DishResponse
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price,
                Description = d.Description,
                IsPopular = d.IsPopular,
                ImageUrl = d.ImageUrl
            });
        }
    }

}
