using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!SessionHelper.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.HoTen = SessionHelper.GetHoTen(HttpContext);
            ViewBag.VaiTro = SessionHelper.GetRole(HttpContext);

            return View();
        }

        public IActionResult Privacy()
        {
            if (!SessionHelper.IsLoggedIn(HttpContext))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}