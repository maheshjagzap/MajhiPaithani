using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajhiPaithani.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class YourMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SBusinessDescription",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SCity",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SPincode",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SProfileImageUrl",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SShopAddress",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SState",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SellerISellerId",
                table: "SellerBankDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Designs",
                columns: table => new
                {
                    IDesignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISellerId = table.Column<int>(type: "int", nullable: false),
                    SDesignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDesignType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BIsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    SellerISellerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designs", x => x.IDesignId);
                    table.ForeignKey(
                        name: "FK_Designs_Sellers_SellerISellerId",
                        column: x => x.SellerISellerId,
                        principalTable: "Sellers",
                        principalColumn: "iSellerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SellerBankDetails_SellerISellerId",
                table: "SellerBankDetails",
                column: "SellerISellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Designs_SellerISellerId",
                table: "Designs",
                column: "SellerISellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerBankDetails_Sellers_SellerISellerId",
                table: "SellerBankDetails",
                column: "SellerISellerId",
                principalTable: "Sellers",
                principalColumn: "iSellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerBankDetails_Sellers_SellerISellerId",
                table: "SellerBankDetails");

            migrationBuilder.DropTable(
                name: "Designs");

            migrationBuilder.DropIndex(
                name: "IX_SellerBankDetails_SellerISellerId",
                table: "SellerBankDetails");

            migrationBuilder.DropColumn(
                name: "SBusinessDescription",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SCity",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SPincode",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SProfileImageUrl",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SShopAddress",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SState",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerISellerId",
                table: "SellerBankDetails");
        }
    }
}
