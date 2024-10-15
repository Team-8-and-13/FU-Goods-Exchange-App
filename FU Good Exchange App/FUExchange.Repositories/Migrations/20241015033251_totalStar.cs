using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUExchange.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class totalStar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalStar",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalStar",
                table: "Products");
        }
    }
}
