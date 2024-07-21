using CSharp5Nhom2.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Text;

namespace CSharp5Nhom2.Controllers
{
    public class SachController : Controller
    {
        DBSach context;
        HttpClient client;
        private readonly IHttpClientFactory _clientFactory;

        public SachController(IHttpClientFactory clientFactory)
        {
            context = new DBSach();
            client = new HttpClient();
            _clientFactory = clientFactory;

        }
        public ActionResult Index()
        {
            string repostURL = "https://localhost:7249/api/Sach/sach-get";
            var repost = client.GetStringAsync(repostURL).Result;
            var data = JsonConvert.DeserializeObject<List<Sach>>(repost);
            return View(data);
        }

        // GET: DongVatController/Details/5
        public ActionResult Details(Guid IDSach)
        {
            string repostURL = $"https://localhost:7249/api/Sach/sach-Get-id?IDSach={IDSach}";
            var repost = client.GetStringAsync(repostURL).Result;
            var data = JsonConvert.DeserializeObject<Sach>(repost);
            return View(data);
        }

        public ActionResult Create()
        {
            ViewBag.TacGiaList = context.tacGias.ToList();
            ViewBag.TheLoaiList = context.theLoais.ToList();
            ViewBag.NhaXuatBanList = context.nhaXuatBans.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sach sach, IFormFile HinhAnh)
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
                context.sachs.Add(sach);
                context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Thêm thất bại. Lỗi: {ex.Message}";
                return View(sach);
            }
        }



        // GET: DongVatController/Edit/5
        public IActionResult Update(Guid IDSach)
        {
            var edit = context.sachs.Find(IDSach);
            ViewBag.TacGiaList = context.tacGias.ToList();
            ViewBag.TheLoaiList = context.theLoais.ToList();
            ViewBag.NhaXuatBanList = context.nhaXuatBans.ToList();
            return View(edit);
        }

        [HttpPost]
        public IActionResult Update(Sach sach, IFormFile newHinhAnh, [FromForm] bool changeImage)
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
                sachInDb.Mota = sach.Mota;
                sachInDb.TacGia = tacGia;
                sachInDb.NhaXuatBan = nhaXuatBan;
                sachInDb.TheLoai = theLoai;

                if (changeImage && newHinhAnh != null)
                {
                    // Xử lý logic để lưu ảnh mới
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newHinhAnh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        newHinhAnh.CopyTo(stream);
                    }
                    sachInDb.HinhAnh = fileName;
                }

                context.SaveChanges();
                return RedirectToAction("Index", "Sach");
            }

            return NotFound();
        }





        public ActionResult Delete(Guid IDSach)
        {
            string repostURL = $"https://localhost:7249/api/Sach/sach-delete?IDSach={IDSach}";
            var repost = client.DeleteAsync(repostURL).Result;
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddToCart(string idSach, int soLuong)
        {
            //var username = HttpContext.Session.GetString("login");
            //if (string.IsNullOrEmpty(username))
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Home");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Sử dụng StringContent để truyền nội dung rỗng
                var response = await client.PostAsync($"https://localhost:7249/api/Sach/add-to-cart?idSach={idSach}&soLuong={soLuong}", new StringContent(""));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    // Xử lý lỗi ở đây
                    return RedirectToAction("Index");
                }
            }
        }

    }

}

