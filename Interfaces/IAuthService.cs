using RestaurantBookingSystem.DTOs.Response;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(string username, string password);
        Task AuthenticateAsync(object username, string password);
    }
}
