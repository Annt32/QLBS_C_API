using System.Text.Json.Serialization;

namespace CSharp5Nhom2.Models
{
    public class HoaDonChiTiet
    {
        public Guid IDHoaDonChiTiet { get; set; }
        public Guid IDHD { get; set; }
        public Guid IDSach { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaTien { get; set; }
        [JsonIgnore]
        public virtual HoaDon HoaDon { get; set; }
        [JsonIgnore]
        public virtual Sach Sach { get; set; }
    }

}
