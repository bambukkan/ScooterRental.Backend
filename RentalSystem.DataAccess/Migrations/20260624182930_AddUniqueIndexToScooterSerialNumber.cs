using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToScooterSerialNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Scooters_SerialNumber",
                table: "Scooters",
                column: "SerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Scooters_SerialNumber",
                table: "Scooters");
        }
    }
}
