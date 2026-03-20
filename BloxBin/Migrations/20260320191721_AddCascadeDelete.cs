using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloxBin.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Users_UserId",
                table: "Bins");

            migrationBuilder.DropIndex(
                name: "IX_Bins_UserId",
                table: "Bins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bins");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_OwnerId",
                table: "Bins",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Users_OwnerId",
                table: "Bins",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Users_OwnerId",
                table: "Bins");

            migrationBuilder.DropIndex(
                name: "IX_Bins_OwnerId",
                table: "Bins");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Bins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bins_UserId",
                table: "Bins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bins_Users_UserId",
                table: "Bins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
