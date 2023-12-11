namespace HotelPosSystem.Entities {
    internal class RoomType {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required byte MaxGuests { get; set; }
    }
}
