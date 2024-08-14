using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class productsplatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Platform",
                table: "categories",
                newName: "PlatformId");

            migrationBuilder.AddColumn<int>(
                name: "PlatformId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "PlatformId",
                table: "categories",
                newName: "Platform");
        }
    }
}
