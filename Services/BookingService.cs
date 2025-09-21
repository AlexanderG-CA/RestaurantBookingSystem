using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Models;
using RestaurantBookingSystem.Repositories;

namespace RestaurantBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ITableRepository _tableRepository;

        public BookingService(
            IRepository<Booking> bookingRepository,
            IRepository<Customer> customerRepository,
            ITableRepository tableRepository)
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
            _tableRepository = tableRepository;
        }

        public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
        {
            // Check if customer exists by phone number, if not create new customer
            var customer = (await _customerRepository.GetAllAsync())
                .FirstOrDefault(c => c.PhoneNumber == request.CustomerPhone);

            if (customer == null)
            {
                customer = new Customer { Name = request.CustomerName, PhoneNumber = request.CustomerPhone };
                await _customerRepository.AddAsync(customer);
                await _customerRepository.SaveChangesAsync();
            }

            // Check table availability
            var isAvailable = await _tableRepository.IsTableAvailableAsync(
                request.TableId, request.BookingDate, request.StartTime);

            if (!isAvailable)
                throw new Exception("Table not available at the selected time");

            var booking = new Booking
            {
                BookingDate = request.BookingDate,
                StartTime = request.StartTime,
                NumberOfGuests = request.NumberOfGuests,
                CustomerId = customer.Id,
                TableId = request.TableId
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return new BookingResponse
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                NumberOfGuests = booking.NumberOfGuests,
                TableId = booking.TableId,
                CustomerName = customer.Name,
                CustomerPhone = customer.PhoneNumber
            };
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(b => new BookingResponse
            {
                Id = b.Id,
                BookingDate = b.BookingDate,
                StartTime = b.StartTime,
                NumberOfGuests = b.NumberOfGuests,
                TableId = b.TableId,
                CustomerName = b.Customer.Name,
                CustomerPhone = b.Customer.PhoneNumber
            });
        }

        public async Task<BookingResponse> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return null;

            return new BookingResponse
            {
                Id = booking.Id,
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                NumberOfGuests = booking.NumberOfGuests,
                TableId = booking.TableId,
                CustomerName = booking.Customer.Name,
                CustomerPhone = booking.Customer.PhoneNumber
            };
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) return false;

            _bookingRepository.Delete(booking);
            return await _bookingRepository.SaveChangesAsync();
        }
    }

}
