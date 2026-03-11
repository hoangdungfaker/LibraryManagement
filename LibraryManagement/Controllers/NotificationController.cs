using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private bool DaDangNhap()
        {
            return HttpContext.Session.GetInt32("MaTaiKhoan") != null;
        }

        private bool LaAdminHoacThuThu()
        {
            var role = HttpContext.Session.GetString("VaiTro");
            return role == "Admin" || role == "ThuThu";
        }

        public async Task<IActionResult> Index()
        {
            if (!DaDangNhap())
                return RedirectToAction("Login", "Account");

            var role = HttpContext.Session.GetString("VaiTro");
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;

            if (role == "DocGia")
            {
                var myNotifications = await _notificationService.GetByTaiKhoanAsync(maTaiKhoan);
                return View(myNotifications);
            }

            var allNotifications = await _notificationService.GetAllAsync();
            return View(allNotifications);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!LaAdminHoacThuThu())
                return RedirectToAction("AccessDenied", "Account");

            var vm = new NotificationCreateViewModel
            {
                DanhSachTaiKhoan = await _notificationService.GetAllTaiKhoanAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationCreateViewModel model)
        {
            if (!LaAdminHoacThuThu())
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
            {
                model.DanhSachTaiKhoan = await _notificationService.GetAllTaiKhoanAsync();
                return View(model);
            }

            await _notificationService.CreateAsync(model.MaTaiKhoan, model.NoiDung);
            TempData["Success"] = "Gửi thông báo thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!DaDangNhap())
                return RedirectToAction("Login", "Account");

            var thongBao = await _notificationService.GetByIdAsync(id);
            if (thongBao == null) return NotFound();

            var role = HttpContext.Session.GetString("VaiTro");
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;

            if (role == "DocGia" && thongBao.MaTaiKhoan != maTaiKhoan)
                return RedirectToAction("AccessDenied", "Account");

            if (role == "DocGia" && !thongBao.DaDoc)
            {
                await _notificationService.MarkAsReadAsync(id);
                thongBao = await _notificationService.GetByIdAsync(id);
            }

            return View(thongBao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOverdue()
        {
            if (!LaAdminHoacThuThu())
                return RedirectToAction("AccessDenied", "Account");

            var count = await _notificationService.SendOverdueNotificationsAsync();
            TempData["Success"] = $"Đã gửi {count} thông báo quá hạn.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            if (!DaDangNhap())
                return RedirectToAction("Login", "Account");

            var thongBao = await _notificationService.GetByIdAsync(id);
            if (thongBao == null) return NotFound();

            var role = HttpContext.Session.GetString("VaiTro");
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;

            if (role == "DocGia" && thongBao.MaTaiKhoan != maTaiKhoan)
                return RedirectToAction("AccessDenied", "Account");

            await _notificationService.MarkAsReadAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}