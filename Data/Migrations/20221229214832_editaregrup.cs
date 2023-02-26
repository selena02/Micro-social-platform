using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_social_platform.Data.Migrations
{
    public partial class editaregrup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");
        }
    }
}
