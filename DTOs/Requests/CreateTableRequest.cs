using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs.Requests
{
    public class CreateTableRequest
    {
        [Required]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20)]
        public int Capacity { get; set; }
    }
}
