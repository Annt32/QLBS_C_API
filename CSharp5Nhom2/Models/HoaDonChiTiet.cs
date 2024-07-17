namespace CSharp5Nhom2.Models
{
    public class HoaDonChiTiet
    {
        public Guid IDHoaDonChiTiet { get; set; }
        public Guid IDHD { get; set; }
        public Guid IDSach { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaTien { get; set; }
        public virtual HoaDon HoaDon { get; set; }
        public virtual Sach Sach { get; set; }
    }

}
