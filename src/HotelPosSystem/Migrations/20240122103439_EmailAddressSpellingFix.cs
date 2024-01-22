using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelPosSystem.Migrations {
    /// <inheritdoc />
    public partial class EmailAddressSpellingFix : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "EmailAdress",
                table: "Customers",
                newName: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Customers",
                newName: "EmailAdress");
        }
    }
}
