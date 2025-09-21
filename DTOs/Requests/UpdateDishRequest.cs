using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs.Requests
{
    public class UpdateDishRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsPopular { get; set; }

        [Url]
        public string ImageUrl { get; set; }
    }
}
