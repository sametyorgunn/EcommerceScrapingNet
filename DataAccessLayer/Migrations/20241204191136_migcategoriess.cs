using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class migcategoriess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "N11SubCategories");

            migrationBuilder.DropTable(
                name: "N11Categories");

            migrationBuilder.CreateTable(
                name: "categoriesMarketplace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatformID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoriesMarketplace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subCategoriesMarketplace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PlatformID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subCategoriesMarketplace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subCategoriesMarketplace_categoriesMarketplace_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categoriesMarketplace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subCategoriesMarketplace_CategoryId",
                table: "subCategoriesMarketplace",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subCategoriesMarketplace");

            migrationBuilder.DropTable(
                name: "categoriesMarketplace");

            migrationBuilder.CreateTable(
                name: "N11Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N11Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "N11SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    N11CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_N11SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_N11SubCategories_N11Categories_N11CategoryId",
                        column: x => x.N11CategoryId,
                        principalTable: "N11Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_N11SubCategories_N11CategoryId",
                table: "N11SubCategories",
                column: "N11CategoryId");
        }
    }
}
