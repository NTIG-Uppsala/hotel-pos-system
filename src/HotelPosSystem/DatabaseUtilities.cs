using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal static class DatabaseUtilities {
        private const string SingleRoomName = "Single Room";
        private const string DoubleRoomName = "Double Room";
        private const string TwinRoomName = "Twin Room";

        internal static void SetUpDatabase() {
            using HotelDbContext databaseContext = new();

            databaseContext.Database.Migrate();

            // If there is any data in the database, skip adding new data
            if (!AreAllTablesEmpty(databaseContext)) {
                return;
            }

            RoomType[] roomTypes = CreateRoomTypes();
            databaseContext.RoomTypes.AddRange(roomTypes);
            RoomType singleRoom = roomTypes.First(roomType => roomType.Name == SingleRoomName);
            RoomType doubleRoom = roomTypes.First(roomType => roomType.Name == DoubleRoomName);
            RoomType twinRoom = roomTypes.First(roomType => roomType.Name == TwinRoomName);

            Building[] buildings = CreateBuildings();
            databaseContext.Buildings.AddRange(buildings);

            PriceCategory[] priceCategories = CreatePriceCategories();
            databaseContext.PriceCategories.AddRange(priceCategories);

            Room singleRoom101 = CreateRoom("101", singleRoom, buildings[0], floor: 1, priceCategories[0]);
            Room doubleRoom201 = CreateRoom("201", doubleRoom, buildings[0], floor: 2, priceCategories[1]);
            Room twinRoom202 = CreateRoom("202", twinRoom, buildings[0], floor: 2, priceCategories[0]);
            databaseContext.Rooms.AddRange(singleRoom101, doubleRoom201, twinRoom202);

            Customer robert = CreateCustomer("Robert Robertsson", "robert.robertsson@example.com", "070-1740650");
            Customer kalle = CreateCustomer("Kalle Kallesson", "kalle.kallesson@example.com", "070-1740640");
            databaseContext.Customers.AddRange(robert, kalle);

            Booking robertsBooking = CreateBooking(robert, twinRoom202, new DateOnly(2023, 12, 24), new DateOnly(2024, 1, 3), isPaidFor: false, isCheckedIn: true, "Cleaning crew one hour late");
            Booking robertsSecondBooking = CreateBooking(robert, doubleRoom201, new DateOnly(2023, 12, 27), new DateOnly(2024, 1, 3), isPaidFor: true, isCheckedIn: false);
            Booking kallesBooking = CreateBooking(kalle, twinRoom202, new DateOnly(2024, 2, 1), new DateOnly(2024, 2, 4), isPaidFor: false, isCheckedIn: false);
            databaseContext.Bookings.AddRange(robertsBooking, robertsSecondBooking, kallesBooking);

            ClosedTimeSpan waterDamage = CreateClosedTimeSpan(singleRoom101, new DateOnly(2023, 12, 20), null, "Water damage");
            databaseContext.ClosedTimeSpans.Add(waterDamage);

            databaseContext.SaveChanges();
        }

        public static bool AreAllTablesEmpty(HotelDbContext databaseContext) {
            return !(databaseContext.Rooms.Any()
                     || databaseContext.RoomTypes.Any()
                     || databaseContext.Customers.Any()
                     || databaseContext.Bookings.Any()
                     || databaseContext.Buildings.Any()
                     || databaseContext.ClosedTimeSpans.Any()
                     || databaseContext.PriceCategories.Any());
        }

        private static RoomType[] CreateRoomTypes() {
            RoomType singleRoom = new() {
                Name = SingleRoomName,
                Description = "A room for one with one bed",
                MaxGuests = 1
            };

            RoomType doubleRoom = new() {
                Name = DoubleRoomName,
                Description = "A room for two with one bed",
                MaxGuests = 2
            };

            RoomType twinRoom = new() {
                Name = TwinRoomName,
                Description = "A room for two with two beds",
                MaxGuests = 2
            };

            return [singleRoom, doubleRoom, twinRoom];
        }

        private static Building[] CreateBuildings() {
            Building building = new() {
                Address = "Building 1"
            };

            return [building];
        }

        private static PriceCategory[] CreatePriceCategories() {
            PriceCategory economy = new() {
                PricePerNight = 100
            };

            PriceCategory business = new() {
                PricePerNight = 200
            };

            return [economy, business];
        }

        private static Room CreateRoom(string name, RoomType type, Building building, short floor, PriceCategory priceCategory) {
            Room room = new() {
                Name = name,
                Floor = floor,
                Building = building,
                Type = type,
                PriceCategory = priceCategory
            };

            return room;
        }

        private static Customer CreateCustomer(string name, string mail, string phoneNumber) {
            Customer customer = new() {
                FullName = name,
                EmailAdress = mail,
                PhoneNumber = phoneNumber
            };

            return customer;
        }

        private static Booking CreateBooking(Customer customer, Room room, DateOnly startDate, DateOnly endDate,
                                             bool isPaidFor, bool isCheckedIn, string? comment = null) {
            Booking booking = new() {
                Customer = customer,
                Room = room,
                StartDate = startDate,
                EndDate = endDate,
                IsPaidFor = isPaidFor,
                IsCheckedIn = isCheckedIn,
                Comment = comment
            };

            return booking;
        }

        private static ClosedTimeSpan CreateClosedTimeSpan(Room room, DateOnly startDate, DateOnly? endDate, string? comment = null) {
            ClosedTimeSpan closedTimeSpan = new() {
                Room = room,
                StartDate = startDate,
                EndDate = endDate,
                Comment = comment
            };

            return closedTimeSpan;
        }
    }
}
