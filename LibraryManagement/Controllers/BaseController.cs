using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class BaseController : Controller
    {
        protected bool KiemTraDangNhap()
        {
            return SessionHelper.IsLoggedIn(HttpContext);
        }

        protected IActionResult ChuyenVeLoginNeuChuaDangNhap()
        {
            if (!KiemTraDangNhap())
            {
                return RedirectToAction("Login", "Account");
            }

            return null!;
        }

        protected bool KiemTraVaiTro(params string[] roles)
        {
            var currentRole = SessionHelper.GetRole(HttpContext);
            return roles.Contains(currentRole);
        }
    }
}