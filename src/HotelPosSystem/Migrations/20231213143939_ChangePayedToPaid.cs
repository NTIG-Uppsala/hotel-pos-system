using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelPosSystem.Migrations {
    /// <inheritdoc />
    public partial class ChangePayedToPaid : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "IsPayedFor",
                table: "Bookings",
                newName: "IsPaidFor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "IsPaidFor",
                table: "Bookings",
                newName: "IsPayedFor");
        }
    }
}
