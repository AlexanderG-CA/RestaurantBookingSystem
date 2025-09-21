using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();
        Task<BookingResponse> GetBookingByIdAsync(int id);
        Task<bool> DeleteBookingAsync(int id);
    }
}
