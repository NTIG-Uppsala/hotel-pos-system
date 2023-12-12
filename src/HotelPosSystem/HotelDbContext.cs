using HotelPosSystem.Entities;

using Microsoft.EntityFrameworkCore;

namespace HotelPosSystem {
    internal class HotelDbContext : DbContext {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<ClosedTimeSpan> ClosedTimeSpans { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PriceCategory> PriceCategories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Program.DatabasePath}");
    }
}
