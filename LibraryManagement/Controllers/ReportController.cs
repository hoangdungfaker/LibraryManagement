using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        private bool KiemTraTruyCap()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            return maTaiKhoan != null && vaiTro == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var model = await _reportService.GetTongQuanAsync();
            return View(model);
        }
    }
}