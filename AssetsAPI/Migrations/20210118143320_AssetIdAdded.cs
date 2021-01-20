using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsAPI.Migrations
{
    public partial class AssetIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssetId",
                table: "Assets",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Assets");
        }
    }
}
