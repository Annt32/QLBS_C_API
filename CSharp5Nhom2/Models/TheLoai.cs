namespace CSharp5Nhom2.Models
{
    public class TheLoai
    {
        public string IDTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public virtual List<Sach> Sachs { get; set; }
    }
}
