using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Models;
using RestaurantBookingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAvailabilityService _availabilityService;

        public BookingsController(AppDbContext context, IAvailabilityService availabilityService)
        {
            _context = context;
            _availabilityService = availabilityService;
        }

        // POST api/bookings
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking(BookingRequest request)
        {
            // Check table availability (time + capacity) using the service
            var availability = await _availabilityService.CheckTableAvailabilityAsync(
                request.TableId, request.BookingDate, request.StartTime, request.NumberOfGuests);

            if (!availability.IsAvailable)
                return BadRequest(availability.Reason);

            // Then handle customer and create booking
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == request.CustomerPhone);
            if (customer == null)
            {
                customer = new Customer { Name = request.CustomerName, PhoneNumber = request.CustomerPhone };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // Get table for response data
            var table = await _context.Tables.FindAsync(request.TableId);
            if (table == null)
                return BadRequest("Table not found"); // This should not happen due to availability check

            var booking = new Booking
            {
                BookingDate = request.BookingDate,
                StartTime = request.StartTime,
                NumberOfGuests = request.NumberOfGuests,
                CustomerId = customer.Id,
                TableId = request.TableId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Booking created successfully",
                bookingId = booking.Id,
                customerName = customer.Name,
                tableNumber = table.TableNumber,
                bookingDate = booking.BookingDate.ToString("yyyy-MM-dd"),
                startTime = booking.StartTime.ToString(@"hh\:mm"),
                numberOfGuests = booking.NumberOfGuests
            });
        }

        // GET: api/bookings
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Table)
                .ToListAsync();
            return Ok(bookings);
        }

        // GET api/bookings/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Table)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        // PUT api/bookings/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id) return BadRequest();
            _context.Entry(booking).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bookings.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // DELETE api/bookings/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class BookingRequest
    {
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }
}