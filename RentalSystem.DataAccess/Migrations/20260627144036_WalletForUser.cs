using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WalletForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Wallet",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wallet",
                table: "Users");
        }
    }
}
