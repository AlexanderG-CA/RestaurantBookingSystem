using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;

namespace RestaurantBookingSystem.Repositories
{
    public interface ITableRepository : IRepository<Table>
    {
        Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime date, TimeSpan time, int guests);
        Task<bool> IsTableAvailableAsync(int tableId, DateTime bookingDate, TimeSpan startTime);
    }

    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime date, TimeSpan time, int guests)
        {
            var endTime = time.Add(TimeSpan.FromHours(2));

            return await _context.Tables
                .Where(t => t.Capacity >= guests)
                .Where(t => !t.Bookings.Any(b => b.BookingDate == date &&
                                                b.StartTime < endTime &&
                                                b.StartTime.Add(TimeSpan.FromHours(2)) > time))
                .ToListAsync();
        }

        public Task<bool> IsTableAvailableAsync(int tableId, DateTime bookingDate, TimeSpan startTime)
        {
            throw new NotImplementedException();
        }
    }

}
