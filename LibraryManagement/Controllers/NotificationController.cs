using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen]
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
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
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Create()
        {
            var vm = new NotificationCreateViewModel
            {
                DanhSachTaiKhoan = await _notificationService.GetAllTaiKhoanAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Create(NotificationCreateViewModel model)
        {
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
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> SendOverdue()
        {
            var count = await _notificationService.SendOverdueNotificationsAsync();
            TempData["Success"] = $"Đã gửi {count} thông báo quá hạn.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
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