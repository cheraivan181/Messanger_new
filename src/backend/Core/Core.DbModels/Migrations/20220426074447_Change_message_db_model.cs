using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class Change_message_db_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelivery",
                table: "Messages",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DialogId",
                table: "Messages",
                column: "DialogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_DialogId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Messages",
                newName: "IsDelivery");
        }
    }
}
