using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Administrators.Any())
            {
                context.Administrators.Add(new Administrator
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                });
            }

            if (!context.Dishes.Any())
            {
                context.Dishes.AddRange(
                    new Dish
                    {
                        Name = "Margherita Pizza",
                        Price = 12.99m,
                        Description = "Classic pizza with tomato sauce and mozzarella",
                        IsPopular = true,
                        ImageUrl = "https://example.com/images/margherita.jpg"
                    },
                    new Dish
                    {
                        Name = "Caesar Salad",
                        Price = 8.99m,
                        Description = "Fresh romaine lettuce with Caesar dressing and croutons",
                        IsPopular = false,
                        ImageUrl = "https://example.com/images/caesar.jpg"
                    }
                );
            }

            if (!context.Tables.Any())
            {
                context.Tables.AddRange(
                    new Table { TableNumber = 1, Capacity = 4 },
                    new Table { TableNumber = 2, Capacity = 2 },
                    new Table { TableNumber = 3, Capacity = 6 },
                    new Table { TableNumber = 4, Capacity = 4 },
                    new Table { TableNumber = 5, Capacity = 8 }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
