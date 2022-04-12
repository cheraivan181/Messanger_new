using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class Adddialogrequestindialog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DialogRequestId",
                table: "Dialogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_DialogRequestId",
                table: "Dialogs",
                column: "DialogRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_DialogRequests_DialogRequestId",
                table: "Dialogs",
                column: "DialogRequestId",
                principalTable: "DialogRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_DialogRequests_DialogRequestId",
                table: "Dialogs");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_DialogRequestId",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "DialogRequestId",
                table: "Dialogs");
        }
    }
}
