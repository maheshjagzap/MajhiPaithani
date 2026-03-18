using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajhiPaithani.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class filedaddedIsSellerProfileComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSellerProfileComplete",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSellerProfileComplete",
                table: "Users");
        }
    }
}
