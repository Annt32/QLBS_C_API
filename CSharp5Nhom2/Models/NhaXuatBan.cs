using System.Text.Json.Serialization;

namespace CSharp5Nhom2.Models
{
    public class NhaXuatBan
    {
        public string IDNXB { get; set; }
        public string TenNXB { get; set; }
        [JsonIgnore]
        public virtual List<Sach>? Sachs { get; set; }
    }
}
