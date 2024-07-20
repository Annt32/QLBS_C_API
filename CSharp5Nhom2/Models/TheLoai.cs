using System.Text.Json.Serialization;

namespace CSharp5Nhom2.Models
{
    public class TheLoai
    {
        public string IDTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        [JsonIgnore]
        public virtual List<Sach> Sachs { get; set; }
    }
}
