using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSharp5Nhom2.Migrations
{
    public partial class Sach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nhaXuatBans",
                columns: table => new
                {
                    IDNXB = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNXB = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhaXuatBans", x => x.IDNXB);
                });

            migrationBuilder.CreateTable(
                name: "tacGias",
                columns: table => new
                {
                    IDTacGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenTacGia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tacGias", x => x.IDTacGia);
                });

            migrationBuilder.CreateTable(
                name: "theLoais",
                columns: table => new
                {
                    IDTheLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenTheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theLoais", x => x.IDTheLoai);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    IDUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.IDUser);
                });

            migrationBuilder.CreateTable(
                name: "sachs",
                columns: table => new
                {
                    IDSach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenSach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDTacGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDNXB = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDTheLoai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sachs", x => x.IDSach);
                    table.ForeignKey(
                        name: "FK_sachs_nhaXuatBans_IDNXB",
                        column: x => x.IDNXB,
                        principalTable: "nhaXuatBans",
                        principalColumn: "IDNXB",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sachs_tacGias_IDTacGia",
                        column: x => x.IDTacGia,
                        principalTable: "tacGias",
                        principalColumn: "IDTacGia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sachs_theLoais_IDTheLoai",
                        column: x => x.IDTheLoai,
                        principalTable: "theLoais",
                        principalColumn: "IDTheLoai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gioHangs",
                columns: table => new
                {
                    IDGH = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gioHangs", x => x.IDGH);
                    table.ForeignKey(
                        name: "FK_gioHangs_users_IDUser",
                        column: x => x.IDUser,
                        principalTable: "users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hoaDons",
                columns: table => new
                {
                    IDHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoaDons", x => x.IDHoaDon);
                    table.ForeignKey(
                        name: "FK_hoaDons_users_IDUser",
                        column: x => x.IDUser,
                        principalTable: "users",
                        principalColumn: "IDUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gioHangChiTiets",
                columns: table => new
                {
                    IDGHCT = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDGH = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TenSach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserIDUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gioHangChiTiets", x => x.IDGHCT);
                    table.ForeignKey(
                        name: "FK_gioHangChiTiets_gioHangs_IDGH",
                        column: x => x.IDGH,
                        principalTable: "gioHangs",
                        principalColumn: "IDGH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gioHangChiTiets_sachs_IDSach",
                        column: x => x.IDSach,
                        principalTable: "sachs",
                        principalColumn: "IDSach",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_gioHangChiTiets_users_UserIDUser",
                        column: x => x.UserIDUser,
                        principalTable: "users",
                        principalColumn: "IDUser");
                });

            migrationBuilder.CreateTable(
                name: "hoaDonChiTiets",
                columns: table => new
                {
                    IDHoaDonChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHD = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSach = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoaDonChiTiets", x => x.IDHoaDonChiTiet);
                    table.ForeignKey(
                        name: "FK_hoaDonChiTiets_hoaDons_IDHD",
                        column: x => x.IDHD,
                        principalTable: "hoaDons",
                        principalColumn: "IDHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hoaDonChiTiets_sachs_IDSach",
                        column: x => x.IDSach,
                        principalTable: "sachs",
                        principalColumn: "IDSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gioHangChiTiets_IDGH",
                table: "gioHangChiTiets",
                column: "IDGH");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangChiTiets_IDSach",
                table: "gioHangChiTiets",
                column: "IDSach");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangChiTiets_UserIDUser",
                table: "gioHangChiTiets",
                column: "UserIDUser");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangs_IDUser",
                table: "gioHangs",
                column: "IDUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hoaDonChiTiets_IDHD",
                table: "hoaDonChiTiets",
                column: "IDHD");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDonChiTiets_IDSach",
                table: "hoaDonChiTiets",
                column: "IDSach");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDons_IDUser",
                table: "hoaDons",
                column: "IDUser");

            migrationBuilder.CreateIndex(
                name: "IX_sachs_IDNXB",
                table: "sachs",
                column: "IDNXB");

            migrationBuilder.CreateIndex(
                name: "IX_sachs_IDTacGia",
                table: "sachs",
                column: "IDTacGia");

            migrationBuilder.CreateIndex(
                name: "IX_sachs_IDTheLoai",
                table: "sachs",
                column: "IDTheLoai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gioHangChiTiets");

            migrationBuilder.DropTable(
                name: "hoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "gioHangs");

            migrationBuilder.DropTable(
                name: "hoaDons");

            migrationBuilder.DropTable(
                name: "sachs");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "nhaXuatBans");

            migrationBuilder.DropTable(
                name: "tacGias");

            migrationBuilder.DropTable(
                name: "theLoais");
        }
    }
}
