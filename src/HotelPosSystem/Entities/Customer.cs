namespace HotelPosSystem.Entities {
    internal class Customer {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string EmailAdress { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
