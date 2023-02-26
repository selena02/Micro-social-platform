using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_social_platform.Data.Migrations
{
    public partial class corectez : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_Articles_ArticleId",
                table: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_ArticleId",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "GroupMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "GroupMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_ArticleId",
                table: "GroupMessages",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_Articles_ArticleId",
                table: "GroupMessages",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
