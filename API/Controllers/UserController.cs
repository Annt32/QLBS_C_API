using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("create_user")]
        public ActionResult Create(User users)
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

        public ActionResult Update(User users)
        {
            try
            {
                var item = context.users.Find(users.IDUser);
                item.Username = users.Username;
                item.MatKhau = users.MatKhau;
                item.SDT = users.SDT;
                item.NgaySinh = users.NgaySinh;
                item.Address = users.Address;

                context.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public ActionResult Delete(User users)
        {
            try
            {
                var delete = context.users.Find(users);
                context.Remove(delete);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
