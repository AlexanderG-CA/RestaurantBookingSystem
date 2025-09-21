using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs.Requests
{
    public class CustomerCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
