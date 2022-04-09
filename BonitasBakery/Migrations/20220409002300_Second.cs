using Microsoft.EntityFrameworkCore.Migrations;

namespace BonitasBakery.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FlavorTreats",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlavorTreats_UserId",
                table: "FlavorTreats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlavorTreats_AspNetUsers_UserId",
                table: "FlavorTreats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlavorTreats_AspNetUsers_UserId",
                table: "FlavorTreats");

            migrationBuilder.DropIndex(
                name: "IX_FlavorTreats_UserId",
                table: "FlavorTreats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlavorTreats");
        }
    }
}
