using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HigherOrLower.Migrations
{
    /// <inheritdoc />
    public partial class HigherOrLower_AddIndicatorOfGameTableClosed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanAddNewPlayers",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanAddNewPlayers",
                table: "Games");
        }
    }
}
