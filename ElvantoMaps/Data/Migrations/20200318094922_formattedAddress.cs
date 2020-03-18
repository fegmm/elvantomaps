using Microsoft.EntityFrameworkCore.Migrations;

namespace ElvantoMaps.Data.Migrations
{
    public partial class formattedAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormattedAddress",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormattedAddress",
                table: "Locations");
        }
    }
}
