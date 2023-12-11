using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class HotelDbContext : DbContext {
        public DbSet<RoomType> RoomTypes { get; set; }

        public string DbPath { get; }

        public HotelDbContext() {
            Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            string path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "hotelPosSystem.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
