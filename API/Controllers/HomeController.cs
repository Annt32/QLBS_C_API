using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        DBSach sach;
        public HomeController()
        {
            sach = new DBSach();
        }
        [HttpGet("login")]
        public ActionResult login(string username, string matkhau)
        {
            try
            {
                if (username == null && matkhau == null)
                {
                    return BadRequest(new {Message = "không để trống."});
                }
                else
                {
                    var data = sach.users.FirstOrDefault(p => p.Username == username && p.MatKhau == matkhau);
                    if (data == null)
                    {
            
                        return BadRequest(new {Message = "Tài khoản hoặc mật khẩu không hợp lệ" });
                    }
                    else
                    {


                        return Ok(new { Message = "Đăng nhập thành công", UserID = data.IDUser });
                    }
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = "lỗi.", ex = ex.Message });
            }
        }
    }
}
