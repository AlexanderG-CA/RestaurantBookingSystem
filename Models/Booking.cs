using RestaurantBookingSystem.Data;

namespace RestaurantBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int NumberOfGuests { get; set; }

        // Foreign keys
        public int CustomerId { get; set; }
        public int TableId { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public Table Table { get; set; }
    }

}
