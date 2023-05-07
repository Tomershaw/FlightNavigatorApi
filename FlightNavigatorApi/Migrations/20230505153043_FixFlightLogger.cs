using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightNavigatorApi.Migrations
{
    /// <inheritdoc />
    public partial class FixFlightLogger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "toFlight",
                table: "FlightLogger",
                newName: "ToLeg");

            migrationBuilder.RenameColumn(
                name: "fromFlight",
                table: "FlightLogger",
                newName: "FromLeg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToLeg",
                table: "FlightLogger",
                newName: "toFlight");

            migrationBuilder.RenameColumn(
                name: "FromLeg",
                table: "FlightLogger",
                newName: "fromFlight");
        }
    }
}
