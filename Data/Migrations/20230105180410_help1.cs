using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro_social_platform.Data.Migrations
{
    public partial class help1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_sender_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_receiver_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Has_accepted = table.Column<bool>(type: "bit", nullable: true),
                    FriendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.Id, x.User_sender_id, x.User_receiver_id });
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_User_receiver_id",
                        column: x => x.User_receiver_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_User_receiver_id",
                table: "Friends",
                column: "User_receiver_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}
