using SachModel = CSharp5Nhom2.Models.Sach;
using SachMigration = CSharp5Nhom2.Migrations.Sach;
using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachController : Controller
    {
        DBSach context;
        public SachController()
        {
            context = new DBSach();
        }

        [HttpGet("sach-get")]
        public IActionResult Get()
        {
            return Ok(context.sachs.ToList());
        }

        // GET api/<UserController>/5
        [HttpGet("sach-Get-id")]
        public IActionResult Get(Guid IDSach)
        {
            var sach = context.sachs.Find(IDSach);
            if (sach == null)
            {
                return NotFound("User not found");
            }
            return Ok(sach);
        }

        [HttpPost("sach-post")]
        public ActionResult Post([FromBody] SachModel sach, IFormFile HinhAnh)
        {
            try
            {
                ViewBag.TacGiaList = context.tacGias.ToList();
                ViewBag.TheLoaiList = context.theLoais.ToList();
                ViewBag.NhaXuatBanList = context.nhaXuatBans.ToList();

                var existingSach = context.sachs.FirstOrDefault(p => p.IDSach == sach.IDSach);
                if (existingSach != null)
                {
                    ModelState.AddModelError("IDSach", "Sách đã tồn tại. Vui lòng chọn tên khác.");
                    return View(sach);
                }

                // Lưu hình ảnh vào thư mục wwwroot/img
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        HinhAnh.CopyTo(stream);
                    }
                    sach.HinhAnh = fileName;
                }

                // Kiểm tra xem IDTacGia, IDNXB, IDTheLoai có tồn tại trong cơ sở dữ liệu hay không
                var tacGia = context.tacGias.FirstOrDefault(t => t.IDTacGia == sach.IDTacGia);
                var nhaXuatBan = context.nhaXuatBans.FirstOrDefault(n => n.IDNXB == sach.IDNXB);
                var theLoai = context.theLoais.FirstOrDefault(l => l.IDTheLoai == sach.IDTheLoai);

                if (tacGia == null || nhaXuatBan == null || theLoai == null)
                {
                    // Nếu không tìm thấy thông tin của các khóa phụ, trả về lỗi
                    ModelState.AddModelError("", "Thông tin khóa phụ không hợp lệ.");
                    return View(sach);
                }

                // Thêm sách vào cơ sở dữ liệu
                context.sachs.Add(new Sach
                {
                    IDSach = sach.IDSach,
                    TenSach = sach.TenSach,
                    IDTacGia = sach.IDTacGia,
                    IDNXB = sach.IDNXB,
                    IDTheLoai = sach.IDTheLoai,
                    Gia = sach.Gia,
                    SoLuong = sach.SoLuong,
                    HinhAnh = sach.HinhAnh,
                    Mota = sach.Mota
                });
                context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Thêm thất bại. Lỗi: {ex.Message}";
                return View(sach);
            }
        }



        // PUT api/<UserController>/5
        [HttpPut("sach-put")]
        public IActionResult Put(SachModel sach)
        {
            var tacGia = context.tacGias.FirstOrDefault(tg => tg.IDTacGia == sach.IDTacGia);
            var nhaXuatBan = context.nhaXuatBans.FirstOrDefault(nxb => nxb.IDNXB == sach.IDNXB);
            var theLoai = context.theLoais.FirstOrDefault(tl => tl.IDTheLoai == sach.IDTheLoai);

            var sachInDb = context.sachs.FirstOrDefault(s => s.IDSach == sach.IDSach);

            if (sachInDb != null)
            {
                sachInDb.TenSach = sach.TenSach;
                sachInDb.Gia = sach.Gia;
                sachInDb.SoLuong = sach.SoLuong;
                sachInDb.HinhAnh = sach.HinhAnh;
                sachInDb.Mota = sach.Mota;
                sachInDb.TacGia = tacGia;
                sachInDb.NhaXuatBan = nhaXuatBan;
                sachInDb.TheLoai = theLoai;

                context.SaveChanges();
                return RedirectToAction("Index", "Sach");
            }

            return NotFound();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("sach-delete")]
        public IActionResult Delete(Guid IDSach)
        {
            var delete = context.sachs.Find(IDSach);
            context.Remove(delete);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost("add-to-cart")]
        [Authorize]
        public IActionResult AddToCart(string idSach, int soLuong)
        {
            try
            {
                if (!Guid.TryParse(idSach, out Guid sachId))
                {
                    return BadRequest("ID sản phẩm không hợp lệ");
                }

                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized();
                }

                var user = context.users.FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    return BadRequest("Người dùng không tồn tại");
                }

                var gioHang = context.gioHangs.FirstOrDefault(gh => gh.IDUser == user.IDUser);
                if (gioHang == null)
                {
                    gioHang = new GioHang
                    {
                        IDGH = Guid.NewGuid(),
                        IDUser = user.IDUser,
                        Status = 1
                    };
                    context.gioHangs.Add(gioHang);
                    context.SaveChanges();
                }

                var sach = context.sachs.FirstOrDefault(s => s.IDSach == sachId);
                if (sach == null)
                {
                    return BadRequest("Sản phẩm không tồn tại");
                }

                var cartItem = context.gioHangChiTiets.FirstOrDefault(p => p.IDSach == sachId && p.IDGH == gioHang.IDGH);
                if (cartItem == null)
                {
                    if (soLuong <= 0)
                    {
                        TempData[$"error_{sachId}"] = "Nhập số lượng cần mua";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (soLuong > sach.SoLuong)
                        {
                            TempData[$"max_{sachId}"] = $"Số lượng trong giỏ hàng đã vượt quá {sach.SoLuong} sản phẩm còn lại";
                            return RedirectToAction("Index", "Home");
                        }

                        GioHangChiTiet cartDetails = new GioHangChiTiet()
                        {
                            IDGHCT = Guid.NewGuid(),
                            IDGH = gioHang.IDGH,
                            IDSach = sachId,
                            SoLuong = soLuong,
                            TenSach = sach.TenSach,
                            HinhAnh = sach.HinhAnh,
                            Gia = sach.Gia,
                        };
                        context.gioHangChiTiets.Add(cartDetails);
                    }
                }
                else
                {
                    if (cartItem.SoLuong + soLuong > sach.SoLuong)
                    {
                        TempData[$"max_{sachId}"] = $"Số lượng trong giỏ hàng đã vượt quá {sach.SoLuong} sản phẩm còn lại";
                        return RedirectToAction("Index", "Home");
                    }

                    cartItem.SoLuong += soLuong;
                }

                context.SaveChanges();
                return Ok(new { message = "Thêm vào giỏ hàng thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
                TempData["ErrorMessage"] = $"Thêm vào giỏ hàng thất bại. Lỗi: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
