using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUExchange.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addCommentArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentArea",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentArea",
                table: "Comments");
        }
    }
}
