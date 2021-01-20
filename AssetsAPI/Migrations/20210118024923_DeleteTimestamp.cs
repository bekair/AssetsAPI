using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsAPI.Migrations
{
    public partial class DeleteTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Assets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Assets",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
