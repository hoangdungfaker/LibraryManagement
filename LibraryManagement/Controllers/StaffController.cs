using LibraryManagement.Models.Entities;
using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class StaffController : Controller
    {
        private readonly StaffService _staffService;

        public StaffController(StaffService staffService)
        {
            _staffService = staffService;
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

            var data = await _staffService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var vm = new StaffFormViewModel
            {
                DanhSachTaiKhoan = await _staffService.GetTaiKhoanChuaGanNhanVienAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffFormViewModel model)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
            {
                model.DanhSachTaiKhoan = await _staffService.GetTaiKhoanChuaGanNhanVienAsync();
                return View(model);
            }

            var nhanVien = new NhanVien
            {
                MaTaiKhoan = model.MaTaiKhoan,
                ChucVu = model.ChucVu
            };

            await _staffService.AddAsync(nhanVien);
            TempData["Success"] = "Thêm nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var nhanVien = await _staffService.GetByIdAsync(id);
            if (nhanVien == null) return NotFound();

            var taiKhoan = await _staffService.GetTaiKhoanChuaGanNhanVienAsync();
            var tkHienTai = nhanVien.TaiKhoan;
            if (tkHienTai != null && !taiKhoan.Any(x => x.MaTaiKhoan == tkHienTai.MaTaiKhoan))
            {
                taiKhoan.Insert(0, tkHienTai);
            }

            var vm = new StaffFormViewModel
            {
                MaNhanVien = nhanVien.MaNhanVien,
                MaTaiKhoan = nhanVien.MaTaiKhoan,
                ChucVu = nhanVien.ChucVu ?? string.Empty,
                DanhSachTaiKhoan = taiKhoan
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffFormViewModel model)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
            {
                model.DanhSachTaiKhoan = await _staffService.GetTaiKhoanChuaGanNhanVienAsync();
                return View(model);
            }

            var nhanVien = await _staffService.GetByIdAsync(model.MaNhanVien);
            if (nhanVien == null) return NotFound();

            nhanVien.MaTaiKhoan = model.MaTaiKhoan;
            nhanVien.ChucVu = model.ChucVu;

            await _staffService.UpdateAsync(nhanVien);
            TempData["Success"] = "Cập nhật nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var nhanVien = await _staffService.GetByIdAsync(id);
            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var nhanVien = await _staffService.GetByIdAsync(id);
            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var result = await _staffService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Không thể xoá nhân viên vì đã có phiếu mượn liên quan.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Xoá nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}