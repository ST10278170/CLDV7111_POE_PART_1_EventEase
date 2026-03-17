namespace CLDV7111_POE_PART_1_EventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }

        // Foreign Keys
        public int EventId { get; set; }
        public int VenueId { get; set; }

        // Navigation
        public Event? Event { get; set; }
        public Venue? Venue { get; set; }
    }
}


