namespace RestaurantBookingSystem.DTOs.Response
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
