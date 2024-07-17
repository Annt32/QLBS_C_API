using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CSharp5Nhom2.Controllers
{
    public class CartController : Controller
    {
        DBSach context;
        public CartController()
        {
            context = new DBSach();
        }
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("login");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            var user = context.users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return BadRequest("Người dùng không tồn tại");
            }

            var cartItems = context.gioHangChiTiets
                .Include(c => c.Sach)
                .Where(c => c.GioHang.User.IDUser == user.IDUser)
                .ToList();

            foreach (var item in cartItems)
            {
                var maxStock = item.Sach.SoLuong;
                if (item.SoLuong > maxStock)
                {
                    item.SoLuong = maxStock;
                    context.SaveChanges();
                }
            }

            return View(cartItems);
        }

        public IActionResult Delete(Guid IDGHCT)
        {
            var find = context.gioHangChiTiets.Find(IDGHCT);
            context.gioHangChiTiets.Remove(find);
            context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult UpdateCart([FromBody] Dictionary<string, int> quantities)
        {
            foreach (var item in quantities)
            {
                if (item.Key.StartsWith("quant_"))
                {
                    Guid id = Guid.Parse(item.Key.Split('_')[1]);
                    int newQuantity = item.Value;
                    var cartItem = context.gioHangChiTiets
                        .Include(ci => ci.Sach)
                        .FirstOrDefault(ci => ci.IDGHCT == id);

                    if (cartItem != null)
                    {
                        var maxStock = cartItem.Sach.SoLuong;
                        if (newQuantity > maxStock)
                        {
                            newQuantity = maxStock;
                        }

                        cartItem.SoLuong = newQuantity;

                        cartItem.Gia = cartItem.Sach.Gia;

                        context.SaveChanges();
                    }
                }
            }
            return Json(new { success = true });
        }
    }
}
