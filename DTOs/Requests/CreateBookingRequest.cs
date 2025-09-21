using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs.Requests
{
    public class CreateBookingRequest
    {
        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Range(1, 50)]
        public int NumberOfGuests { get; set; }

        [Required]
        public int TableId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhone { get; set; }
    }
}
