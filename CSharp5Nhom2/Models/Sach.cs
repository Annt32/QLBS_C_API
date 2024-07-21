using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CSharp5Nhom2.Models
{
    public class Sach
    {
        public Guid IDSach { get; set; }
        public string TenSach { get; set; }
        public string IDTacGia { get; set; }
        public string IDNXB { get; set; }
        public string IDTheLoai { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public string Mota { get; set; }
        [JsonIgnore]
        public virtual TacGia? TacGia { get; set; }
        [JsonIgnore]
        public virtual NhaXuatBan? NhaXuatBan { get; set; }
        [JsonIgnore]
        public virtual TheLoai? TheLoai { get; set; }
        [JsonIgnore]
        public virtual List<HoaDonChiTiet>? HoaDonChiTiets { get; set; } // Thay đổi thành ICollection
    }

}
