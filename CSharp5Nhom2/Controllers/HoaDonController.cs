using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;

namespace CSharp5Nhom2.Controllers
{
    public class HoaDonController : Controller
    {
        DBSach _context;

        public HoaDonController()
        {
            _context = new DBSach();
        }

        public IActionResult Index()
        {
            return View();
        }


        //public IActionResult Purchase(string selectedItems)
        //{
        //    if (string.IsNullOrEmpty(selectedItems))
        //    {
        //        TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
        //        return RedirectToAction("Index", "Cart");
        //    }

        //    var selectedIds = selectedItems.Split(',').Select(id =>
        //    {
        //        Guid.TryParse(id, out var parsedId);
        //        return parsedId;
        //    }).ToList();

        //    var selectedItemsInCart = _context.gioHangChiTiets
        //        .Include(ghct => ghct.Sach) // Sử dụng Include để nạp đối tượng liên quan
        //        .Where(ghct => selectedIds.Contains(ghct.IDGHCT))
        //        .ToList();

        //    if (!selectedItemsInCart.Any())
        //    {
        //        TempData["ErrorMessage"] = "Một hoặc nhiều mục đã được chọn không hợp lệ.";
        //        return RedirectToAction("Index", "Cart");
        //    }

        //    var user = HttpContext.Session.GetString("login");
        //    if (string.IsNullOrEmpty(user))
        //    {
        //        TempData["ErrorMessage"] = "Không thể xác định người dùng.";
        //        return RedirectToAction("Index", "Cart");
        //    }

        //    var userID = _context.users.FirstOrDefault(u => u.Username == user);
        //    if (userID == null)
        //    {
        //        TempData["ErrorMessage"] = "Không thể xác định người dùng.";
        //        return RedirectToAction("Index", "Cart");
        //    }

        //    var hoaDon = new HoaDon
        //    {
        //        IDHoaDon = Guid.NewGuid(),
        //        IDUser = userID.IDUser,
        //        NgayTao = DateTime.Now,
        //        SoLuong = selectedItemsInCart.Sum(item => item.SoLuong)
        //    };

        //    _context.hoaDons.Add(hoaDon);

        //    foreach (var item in selectedItemsInCart)
        //    {

        //        var hdct = new HoaDonChiTiet
        //        {
        //            IDHoaDonChiTiet = Guid.NewGuid(),
        //            IDHD = hoaDon.IDHoaDon,
        //            IDSach = item.IDSach,
        //            SoLuong = item.SoLuong,
        //            GiaTien = item.Sach.Gia
        //        };
        //        _context.hoaDonChiTiets.Add(hdct);

        //        var sach = _context.sachs.FirstOrDefault(s => s.IDSach == item.IDSach);
        //        if (sach != null)
        //        {
        //            sach.SoLuong -= item.SoLuong;
        //        }

        //        _context.gioHangChiTiets.Remove(item);
        //    }

        //    _context.SaveChanges();

        //    return RedirectToAction("Details", new { id = hoaDon.IDHoaDon });
        //}



        public IActionResult Purchase(string selectedItems)
        {
            if (string.IsNullOrEmpty(selectedItems))
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.";
                return RedirectToAction("Index", "Cart");
            }

            var selectedIds = selectedItems.Split(',').Select(Guid.Parse).ToList();
            var selectedItemsInCart = _context.gioHangChiTiets
                .Include(ghct => ghct.Sach) // Sử dụng Include để nạp đối tượng liên quan
                .Where(ghct => selectedIds.Contains(ghct.IDGHCT))
                .ToList();

            if (!selectedItemsInCart.Any())
            {
                TempData["ErrorMessage"] = "Một hoặc nhiều mục đã được chọn không hợp lệ.";
                return RedirectToAction("Index", "Cart");
            }

            var user = HttpContext.Session.GetString("login");
            var userID = _context.users.FirstOrDefault(u => u.Username == user);

            if (userID == null)
            {
                TempData["ErrorMessage"] = "Không thể xác định người dùng.";
                return RedirectToAction("Index", "Cart");
            }

            var hoaDon = new HoaDon
            {
                IDHoaDon = Guid.NewGuid(),
                IDUser = userID.IDUser,
                NgayTao = DateTime.Now,
                SoLuong = selectedItemsInCart.Sum(item => item.SoLuong),
                TrangThai = 0
            };

            _context.hoaDons.Add(hoaDon);

            foreach (var item in selectedItemsInCart)
            {
                if (item.Sach == null)
                {
                    TempData["ErrorMessage"] = "Một hoặc nhiều sản phẩm không tồn tại.";
                    return RedirectToAction("Index", "Cart");
                }

                HoaDonChiTiet hdct = new HoaDonChiTiet
                {
                    IDHoaDonChiTiet = Guid.NewGuid(),
                    IDHD = hoaDon.IDHoaDon,
                    IDSach = item.IDSach,
                    SoLuong = item.SoLuong,
                    GiaTien = item.Sach.Gia
                };
                _context.hoaDonChiTiets.Add(hdct);

                var sach = _context.sachs.FirstOrDefault(s => s.IDSach == item.IDSach);
                if (sach != null)
                {
                    sach.SoLuong -= item.SoLuong;
                }

                _context.gioHangChiTiets.Remove(item);
            }


            _context.SaveChanges();

            return RedirectToAction("Details", "ThongKe", new { id = hoaDon.IDHoaDon });
        }

        public IActionResult ThanhToanHoaDon(Guid id)
        {
            var hoaDon = _context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.Sach)
                .FirstOrDefault(hd => hd.IDHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            if (hoaDon.TrangThai != 0)
            {
                TempData["ErrorMessage"] = "Hóa đơn này không thể thanh toán.";
                return RedirectToAction("Details", "ThongKe", new { id = hoaDon.IDHoaDon });
            }
           
            hoaDon.TrangThai = 1;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Hóa đơn đã được thanh toán thành công.";
            return RedirectToAction("Details", "ThongKe", new { id = hoaDon.IDHoaDon });
        }


        public IActionResult HuyHoaDon(Guid id)
        {
            var hoaDon = _context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .FirstOrDefault(hd => hd.IDHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái hóa đơn về 100 (hủy hóa đơn)
            hoaDon.TrangThai = 100;

            // Hoàn lại số lượng sản phẩm về kho
            foreach (var item in hoaDon.HoaDonChiTiets)
            {
                var sach = _context.sachs.FirstOrDefault(s => s.IDSach == item.IDSach);
                if (sach != null)
                {
                    sach.SoLuong += item.SoLuong;
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Details", "ThongKe", new { id = hoaDon.IDHoaDon });
        }



        public IActionResult MuaLai(Guid id)
        {
            var hoaDon = _context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.Sach)
                .FirstOrDefault(hd => hd.IDHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            var username = HttpContext.Session.GetString("login");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var user = _context.users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return BadRequest("Người dùng không tồn tại");
            }

            var gioHang = _context.gioHangs.FirstOrDefault(gh => gh.IDUser == user.IDUser);
            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    IDGH = Guid.NewGuid(),
                    IDUser = user.IDUser,
                    Status = 1
                };
                _context.gioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            foreach (var item in hoaDon.HoaDonChiTiets)
            {
                var sach = item.Sach;
                var cartItem = _context.gioHangChiTiets.FirstOrDefault(p => p.IDSach == item.IDSach && p.IDGH == gioHang.IDGH);
                if (cartItem == null)
                {
                    if (item.SoLuong > sach.SoLuong)
                    {
                        TempData["ErrorMessage"] = $"Số lượng trong giỏ hàng đã vượt quá {sach.SoLuong} sản phẩm còn lại";
                        return RedirectToAction("Index", "Home");
                    }

                    GioHangChiTiet cartDetails = new GioHangChiTiet()
                    {
                        IDGHCT = Guid.NewGuid(),
                        IDGH = gioHang.IDGH,
                        IDSach = item.IDSach,
                        SoLuong = item.SoLuong,
                        TenSach = sach.TenSach,
                        HinhAnh = sach.HinhAnh,
                        Gia = sach.Gia,
                    };
                    _context.gioHangChiTiets.Add(cartDetails);
                }
                else
                {
                    if (cartItem.SoLuong + item.SoLuong > sach.SoLuong)
                    {
                        TempData["ErrorMessage"] = $"Số lượng trong giỏ hàng đã vượt quá {sach.SoLuong} sản phẩm còn lại";
                        return RedirectToAction("Index", "Home");
                    }

                    cartItem.SoLuong += item.SoLuong;
                }
            }

            // Cập nhật trạng thái hóa đơn về 100 (Hủy hóa đơn)
            
            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }





        public IActionResult Details(Guid id)
        {
            var hoaDon = _context.hoaDons
                .Include(hd => hd.HoaDonChiTiets)
                .ThenInclude(hdct => hdct.Sach)
                .FirstOrDefault(hd => hd.IDHoaDon == id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }
    }
}
