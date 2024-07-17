using System.ComponentModel.DataAnnotations;

namespace CSharp5Nhom2.Models
{
    public class User
    {
        public Guid IDUser { get; set; }
        [Required]
        [StringLength(450, MinimumLength = 6, ErrorMessage = "Độ dài user tối thiểu 6 kí tự")]
        public string Username { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[!@#$%^&*])(?=.*[a-zA-Z])(?=.*\d).{6,}$", ErrorMessage = "Độ dài mật khẩu tối thiểu 6 ký tự và bao gồm ít nhất một ký tự đặc biệt")]
        public string MatKhau { get; set; }
        [RegularExpression("^0\\d{9}$",
        ErrorMessage = "Số điện thoại phải đúng format và có 10 chữ số")]
        public string SDT { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Address { get; set; }
        public virtual List<HoaDon> HoaDons { get; set; }
        public virtual GioHang GioHang { get; set; }
        public virtual List<GioHangChiTiet> GioHangChiTiets { get; set; }

    }
}
