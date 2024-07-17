using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSharp5Nhom2.Migrations
{
    public partial class AddTrangThaiToHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "hoaDons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "hoaDons");
        }
    }
}
