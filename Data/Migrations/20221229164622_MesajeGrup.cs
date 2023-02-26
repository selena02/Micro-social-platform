using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_social_platform.Data.Migrations
{
    public partial class MesajeGrup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "GroupMessages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GroupMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_ArticleId",
                table: "GroupMessages",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_UserId",
                table: "GroupMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_Articles_ArticleId",
                table: "GroupMessages",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMessages_AspNetUsers_UserId",
                table: "GroupMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_Articles_ArticleId",
                table: "GroupMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMessages_AspNetUsers_UserId",
                table: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_ArticleId",
                table: "GroupMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_UserId",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "GroupMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupMessages");
        }
    }
}
