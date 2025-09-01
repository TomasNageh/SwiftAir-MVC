using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwiftAir.Migrations
{
    /// <inheritdoc />
    public partial class AddingImagePathCoulmnToFlightTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Flights");
        }
    }
}
