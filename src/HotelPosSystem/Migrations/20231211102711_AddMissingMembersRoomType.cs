using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelPosSystem.Migrations {
    /// <inheritdoc />
    public partial class AddMissingMembersRoomType : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoomTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "MaxGuests",
                table: "RoomTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "MaxGuests",
                table: "RoomTypes");
        }
    }
}
