namespace HotelPosSystem.Entities {
    internal class Booking {
        public int Id { get; set; }
        public required Customer Customer { get; set; }
        public required Room Room { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required bool IsPayedFor { get; set; }
        public required bool IsCheckedIn { get; set; }
        public string? Comment { get; set; }
    }
}
