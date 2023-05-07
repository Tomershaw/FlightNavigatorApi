using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightNavigatorApi.Migrations
{
    /// <inheritdoc />
    public partial class AddLegsToLogeer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextFlight",
                table: "FlightLogger",
                newName: "toFlight");

            migrationBuilder.AddColumn<int>(
                name: "fromFlight",
                table: "FlightLogger",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fromFlight",
                table: "FlightLogger");

            migrationBuilder.RenameColumn(
                name: "toFlight",
                table: "FlightLogger",
                newName: "NextFlight");
        }
    }
}
