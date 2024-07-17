using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSharp5Nhom2.Controllers
{
    public class ThongKeController : Controller
    {
        DBSach context;
        public ThongKeController()
        {
            context = new DBSach();
        }
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("login");
            if (string.IsNullOrEmpty(user))
            {
                TempData["ErrorMessage"] = "Không thể xác định người dùng.";
                return RedirectToAction("Index", "Home");
            }

            var userID = context.users.FirstOrDefault(u => u.Username == user);
            if (userID == null)
            {
                TempData["ErrorMessage"] = "Không thể xác định người dùng.";
                return RedirectToAction("Index", "Home");
            }

            var hoaDons = await context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .Where(hd => hd.IDUser == userID.IDUser)
                .ToListAsync();

            return View(hoaDons);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var hoaDon = await context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.Sach)
                .FirstOrDefaultAsync(hd => hd.IDHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Tính tổng tiền
            hoaDon.TongTien = hoaDon.HoaDonChiTiets.Sum(hdct => hdct.GiaTien * hdct.SoLuong);

            return View(hoaDon);
        }


    }
}
