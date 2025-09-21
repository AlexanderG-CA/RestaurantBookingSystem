using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Table> Tables { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Dish> Dishes { get; }
        IRepository<Administrator> Administrators { get; }
        Task<bool> CompleteAsync();
    }
}
