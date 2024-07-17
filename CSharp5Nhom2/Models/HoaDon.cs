using System.ComponentModel.DataAnnotations.Schema;

namespace CSharp5Nhom2.Models
{
    public class HoaDon
    {
        public Guid IDHoaDon { get; set; }
        public Guid IDUser { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuong { get; set; }
        public virtual User User { get; set; }
        public virtual List<HoaDonChiTiet> HoaDonChiTiets { get; set; }

        public int TrangThai { get; set; }

        [NotMapped]
        public decimal TongTien { get; set; }

    }
}
