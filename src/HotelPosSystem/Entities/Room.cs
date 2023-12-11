namespace HotelPosSystem.Entities {
    internal class Room {
        public int Id { get; set; }
        public required RoomType Type { get; set; }
        public required string Name { get; set; }
        public required Building Building { get; set; }
        public required short Floor { get; set; }
        public required PriceCategory PriceCategory { get; set; }
        public string? Description { get; set; }
        public string? Comment { get; set; }
    }
}
