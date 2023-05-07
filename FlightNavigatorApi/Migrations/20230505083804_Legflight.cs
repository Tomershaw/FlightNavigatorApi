using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightNavigatorApi.Migrations
{
    /// <inheritdoc />
    public partial class Legflight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Leg",
                table: "Flight",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leg",
                table: "Flight");
        }
    }
}
