namespace RestaurantBookingSystem.DTOs.Response
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }

}
