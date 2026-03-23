using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajhiPaithani.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "bIsDelivered",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "bIsRead",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "dDeliveredDate",
                table: "ChatMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dReadDate",
                table: "ChatMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "iReceiverUserId",
                table: "ChatMessages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bIsDelivered",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "bIsRead",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "dDeliveredDate",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "dReadDate",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "iReceiverUserId",
                table: "ChatMessages");
        }
    }
}
