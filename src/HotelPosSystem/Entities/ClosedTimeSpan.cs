using System.Security.Permissions;

namespace HotelPosSystem.Entities {
    internal class ClosedTimeSpan {
        public int Id { get; set; }
        public required Room Room { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Comment { get; set; }

    }
}
