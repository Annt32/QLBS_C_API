using CSharp5Nhom2.Iservers;
using CSharp5Nhom2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;

namespace CSharp5Nhom2.Controllers
{
    public class UserController : Controller
    {
        HttpClient client;

        public UserController()
        {
            client = new HttpClient();
        }
        public ActionResult Index()
        {
            string repostURL = "https://localhost:7249/api/User/nv-get";
            var repost = client.GetStringAsync(repostURL).Result;
            var data = JsonConvert.DeserializeObject<List<User>>(repost);
            return View(data);
        }

        // GET: DongVatController/Details/5
        public ActionResult Details(Guid id)
        {
            string repostURL = $"https://localhost:7249/api/User/nv-Get-id?id={id}";
            var repost = client.GetStringAsync(repostURL).Result;
            var data = JsonConvert.DeserializeObject<User>(repost);
            return View(data);
        }

        // GET: DongVatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DongVatController/Create
        public ActionResult CreateNV(User users)
        {
            try
            {
                string repostURL = $"https://localhost:7249/api/User/nv-post";
                var repost = client.PostAsJsonAsync(repostURL, users).Result;
                return RedirectToAction("Index");
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: DongVatController/Edit/5
        public ActionResult Edit(string id)
        {
            string repostURL = $"https://localhost:7249/api/User/nv-Get-id?id={id}";
            var repost = client.GetStringAsync(repostURL).Result;
            var data = JsonConvert.DeserializeObject<User>(repost);
            return View(data);
        }

        // POST: DongVatController/Edit/5
        [HttpPost]
        public ActionResult Edit(User users)
        {
            try
            {
                string repostURL = $"https://localhost:7249/api/User/nv-put";
                var repost = client.PutAsJsonAsync(repostURL, users).Result;
                return RedirectToAction("Index");
            }
            catch
            {
                return BadRequest();
            }
        }

        public ActionResult Delete(Guid id)
        {
            string repostURL = $"https://localhost:7249/api/User/nv-delete?id={id}";
            var repost = client.DeleteAsync(repostURL).Result;
            return RedirectToAction("Index");
        }

      
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult SignUpNV(User user)
        {
            try
            {
                string repostURL = $"https://localhost:7249/api/User/nv-SignUp";
                var repost = client.PostAsJsonAsync(repostURL, user).Result;
                return RedirectToAction("Index");
            }
            catch
            {
                return BadRequest();
            }

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }

    }
}
