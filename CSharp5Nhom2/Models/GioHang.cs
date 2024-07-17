namespace CSharp5Nhom2.Models
{
    public class GioHang
    {
        public Guid IDGH { get; set; }
        public Guid IDUser { get; set; }
        public int Status { get; set; }
        public virtual User User { get; set; }
        public virtual List<GioHangChiTiet> GioHangChiTiets { get; set; }
    }
}
