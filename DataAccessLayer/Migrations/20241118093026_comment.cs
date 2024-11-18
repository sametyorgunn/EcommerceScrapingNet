using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class comment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trendyolCategories");

            migrationBuilder.AddColumn<string>(
                name: "ProductPlatformID",
                table: "comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPlatformID",
                table: "comments");

            migrationBuilder.CreateTable(
                name: "trendyolCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TrendyolCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trendyolCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trendyolCategories_trendyolCategories_TrendyolCategoryId",
                        column: x => x.TrendyolCategoryId,
                        principalTable: "trendyolCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_trendyolCategories_TrendyolCategoryId",
                table: "trendyolCategories",
                column: "TrendyolCategoryId");
        }
    }
}
