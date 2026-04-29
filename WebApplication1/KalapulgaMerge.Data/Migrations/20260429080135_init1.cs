using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KalapulgaMerge.Data.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShopItems",
                columns: new[] { "Id", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "Type" },
                values: new object[,]
                {
                    { 1, "", "lib/defaultassets/image/shop1.png", true, "Punane müts", 500m, 0 },
                    { 2, "", "lib/defaultassets/image/shop1.png", true, "Sinine müts", 500m, 0 },
                    { 3, "", "lib/defaultassets/image/shop2.png", true, "Ümarad prillid", 750m, 1 },
                    { 4, "", "lib/defaultassets/image/shop2.png", true, "Päikeseprillid", 750m, 1 },
                    { 5, "", "", true, "Kuldne värv", 1000m, 2 },
                    { 6, "", "", true, "Hõbedane värv", 1000m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopItems");
        }
    }
}
