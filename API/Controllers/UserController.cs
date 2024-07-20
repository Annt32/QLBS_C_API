using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        DBSach context;
        public UserController()
        {
            context = new DBSach();
        }

        [HttpGet("nv-get")]
        public IActionResult Get()
        {
            return Ok(context.users.ToList());
        }

        // GET api/<UserController>/5
        [HttpGet("nv-Get-id")]
        public IActionResult Get(Guid id)
        {
            var user = context.users.Find(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost("nv-post")]
        public ActionResult Post(User users)
        {
            try
            {
                users.IDUser = Guid.NewGuid();
                context.users.Add(users);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("nv-put")]
        public IActionResult Put(User users)
        {
            var item = context.users.Find(users.IDUser);
            if (item == null)
            {
                return NotFound("User not found");
            }

            item.Username = users.Username;
            item.MatKhau = users.MatKhau;
            item.SDT = users.SDT;
            item.NgaySinh = users.NgaySinh;
            item.Address = users.Address;
            context.SaveChanges();
            return Ok(new { message = "Cập nhật thành công", user = item });
        }

        // DELETE api/<UserController>/5
        [HttpDelete("nv-delete")]
        public IActionResult Delete(Guid id)
        {
            var delete = context.users.Find(id);
            if (delete == null)
            {
                return NotFound("User not found");
            }

            context.Remove(delete);
            context.SaveChanges();
            return Ok(new { message = "Xóa thành công" });
        }

        [HttpPost("nv-SignUp")]
        public ActionResult SignUp([FromBody] User user)
        {
            try
            {
                // Kiểm tra xem user có hợp lệ không
                if (user == null)
                {
                    return BadRequest(new { message = "Thông tin người dùng không hợp lệ" });
                }

                // Kiểm tra xem username đã tồn tại chưa
                var tontai = context.users.FirstOrDefault(p => p.Username == user.Username);
                if (tontai != null)
                {
                    return BadRequest(new { message = $"Username {user.Username} đã được sử dụng, vui lòng chọn username khác" });
                }

                // Khởi tạo ID cho user nếu chưa có
                if (user.IDUser == Guid.Empty)
                {
                    user.IDUser = Guid.NewGuid();
                }

                // Thêm user vào context
                context.users.Add(user);

                // Tạo đối tượng GioHang
                GioHang giohang = new GioHang()
                {
                    IDGH = Guid.NewGuid(),
                    IDUser = user.IDUser,
                    Status = 1
                };
                context.gioHangs.Add(giohang);

                // Lưu thay đổi vào database
                context.SaveChanges();

                return Ok(new { message = "Tạo tài khoản thành công", user = user });
            }
            catch (Exception ex)
            {
                // Trả về lỗi chi tiết
                return BadRequest(new { message = "Đã có lỗi xảy ra", error = ex.Message });
            }
        }


    }
}
