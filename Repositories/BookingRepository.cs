using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Models;

namespace RestaurantBookingSystem.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date);
        Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan startTime);
    }

    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Table)
                .Where(b => b.BookingDate == date)
                .ToListAsync();
        }

        public async Task<bool> IsTableAvailableAsync(int tableId, DateTime date, TimeSpan startTime)
        {
            var endTime = startTime.Add(TimeSpan.FromHours(2));

            return !await _context.Bookings
                .AnyAsync(b => b.TableId == tableId &&
                              b.BookingDate == date &&
                              b.StartTime < endTime &&
                              b.StartTime.Add(TimeSpan.FromHours(2)) > startTime);
        }
    }
}
