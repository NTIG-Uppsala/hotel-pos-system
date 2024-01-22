namespace HotelPosSystem.Entities {
    internal class Customer {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }

        public override string ToString() {
            return FullName;
        }
    }
}
