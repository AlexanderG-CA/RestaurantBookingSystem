namespace RestaurantBookingSystem.Interfaces
{
    public interface IAvailabilityService
    {
        Task<AvailabilityResult> CheckTableAvailabilityAsync(int tableId, DateTime date, TimeSpan startTime, int numberOfGuests);
    }

    public class AvailabilityResult
    {
        public bool IsAvailable { get; set; }
        public string Reason { get; set; }
    }
}
