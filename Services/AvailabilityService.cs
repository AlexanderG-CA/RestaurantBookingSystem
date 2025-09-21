using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using System;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly AppDbContext _context;

        public AvailabilityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AvailabilityResult> CheckTableAvailabilityAsync(
    int tableId, DateTime date, TimeSpan startTime, int numberOfGuests)
        {
            // Get table & capacity
            var table = await _context.Tables.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tableId);
            if (table == null)
                return new AvailabilityResult { IsAvailable = false, Reason = "Table not found" };
            if (table.Capacity < numberOfGuests)
                return new AvailabilityResult { IsAvailable = false, Reason = "Insufficient capacity" };

            // Build the requested booking’s absolute time window
            var duration = TimeSpan.FromHours(2);
            var newStart = date.Date + startTime;   // exact date + time (no time on the date part)
            var newEnd = newStart.Add(duration);

            // To be safe with midnight crossings and any .Date translation quirks,
            // fetch candidate bookings from the same day, the day before, and the day after.
            var day = date.Date;
            var candidates = await _context.Bookings.AsNoTracking()
                .Where(b => b.TableId == tableId &&
                       (b.BookingDate.Date == day
                     || b.BookingDate.Date == day.AddDays(-1)
                     || b.BookingDate.Date == day.AddDays(1)))
                .ToListAsync();

            // Now apply exact time overlap on the server side
            foreach (var b in candidates)
            {
                var existingStart = b.BookingDate.Date + b.StartTime; // precise start of existing booking
                var existingEnd = existingStart.Add(duration);

                var overlaps = newStart < existingEnd && newEnd > existingStart;
                if (overlaps)
                {
                    var displayEnd = b.StartTime.Add(duration);
                    return new AvailabilityResult
                    {
                        IsAvailable = false,
                        Reason = $"Table already booked from {b.StartTime:hh\\:mm} to {displayEnd:hh\\:mm}"
                    };
                }
            }

            return new AvailabilityResult { IsAvailable = true, Reason = "Available" };
        }
    }
}
