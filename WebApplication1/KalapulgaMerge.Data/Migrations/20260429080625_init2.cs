using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalapulgaMerge.Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop2.png");

            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop3.png");

            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop4.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop1.png");

            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop2.png");

            migrationBuilder.UpdateData(
                table: "ShopItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "lib/defaultassets/image/shop2.png");
        }
    }
}
