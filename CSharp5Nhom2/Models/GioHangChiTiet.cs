namespace CSharp5Nhom2.Models
{
    public class GioHangChiTiet
    {
        public Guid IDGHCT { get; set; }
        public Guid IDGH {  get; set; }
        public Guid IDSach { get; set; }
        public int SoLuong { get; set; }
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        public decimal Gia { get; set; }
        public int Status { get; set; }
        public virtual GioHang GioHang { get; set; }
        public virtual Sach Sach { get; set; }
    }
}
