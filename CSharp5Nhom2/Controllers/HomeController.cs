using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace CSharp5Nhom2.Controllers
{
    public class HomeController : Controller
    {
        DBSach sach;

        public HomeController()
        {
            sach = new DBSach();
        }

        public IActionResult Index()
        {
            var data = sach.sachs.ToList();
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login(string username, string matkhau)
        {
            if (username == null && matkhau == null)
            {
                return View();
            }
            else
            {
                var data = sach.users.FirstOrDefault(p => p.Username == username && p.MatKhau == matkhau);
                if (data == null)
                {
                    TempData["none"] = "Tài khoản hoặc mật khẩu không hợp lệ";
                    return View(data);
                }
                else
                {
                    // Lưu username vào session
                    HttpContext.Session.SetString("login", username);

                    // Lưu IDUser vào session
                    HttpContext.Session.SetString("IDUser", data.IDUser.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult BuyNow()
        {
            try
            {
                // Lấy thông tin người dùng từ session
                var username = HttpContext.Session.GetString("login");
                var user = sach.users.FirstOrDefault(u => u.Username == username);

                // Tạo hóa đơn mới
                var newOrder = new HoaDon
                {
                    IDHoaDon = Guid.NewGuid(),
                    IDUser = user.IDUser,
                    NgayTao = DateTime.Now,
                    SoLuong = 0 // Sẽ được cập nhật sau khi thêm các chi tiết hóa đơn
                };

                // Thêm hóa đơn mới vào cơ sở dữ liệu
                sach.hoaDons.Add(newOrder);
                sach.SaveChanges();

                // Lặp qua các sản phẩm trong giỏ hàng và tạo chi tiết hóa đơn tương ứng
                foreach (var cartItem in user.GioHang.GioHangChiTiets)
                {
                    var newOrderDetail = new HoaDonChiTiet
                    {
                        IDHoaDonChiTiet = Guid.NewGuid(),
                        IDHD = newOrder.IDHoaDon,
                        IDSach = cartItem.Sach.IDSach,
                        SoLuong = cartItem.SoLuong,
                        // Thêm các thông tin khác cần thiết
                    };

                    // Thêm chi tiết hóa đơn mới vào cơ sở dữ liệu
                    sach.hoaDonChiTiets.Add(newOrderDetail);

                    // Cập nhật số lượng sản phẩm trong bảng Sach
                    var total = sach.sachs.FirstOrDefault(s => s.IDSach == cartItem.IDSach);
                    if (total != null)
                    {
                        total.SoLuong -= cartItem.SoLuong;
                    }
                }

                // Xóa các mục trong giỏ hàng của người dùng sau khi thanh toán
                user.GioHang.GioHangChiTiets.Clear();
                sach.SaveChanges();

                // Thực hiện xử lý thanh toán và chuyển hướng đến trang thông báo hoặc xác nhận thanh toán
                return RedirectToAction("PaymentSuccess");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Thanh toán thất bại. Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
