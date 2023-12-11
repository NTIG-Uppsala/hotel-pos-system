namespace HotelPosSystem.Entities {
    internal class Booking {
        public int Id { get; set; }
        public required Customer Customer { get; set; }
        public required Room Room { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsPayedFor { get; set; }
        public bool IsCheckedIn { get; set; }
        public string? Comment { get; set; }
    }
}
