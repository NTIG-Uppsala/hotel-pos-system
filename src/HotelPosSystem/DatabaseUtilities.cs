using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal static class DatabaseUtilities {
        internal static void SetUpDatabase(HotelDbContext databaseContext) {
            databaseContext.Database.Migrate();
            CreateRoomTypesIfEmpty(databaseContext, "Single Room", "Double Room");
        }

        private static void CreateRoomTypesIfEmpty(HotelDbContext databaseContext, params string[] names) {
            if (!databaseContext.RoomTypes.Any()) {
                foreach (string name in names) {
                    RoomType roomType = new() {
                        Name = name
                    };
                    databaseContext.Add(roomType);
                }
                databaseContext.SaveChanges();
            }
        }
    }
}
