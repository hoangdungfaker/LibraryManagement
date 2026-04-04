using LibraryManagement.Models.Entities;
using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen("Admin")]
    public class StaffController : Controller
    {
        private readonly StaffService _staffService;

        public StaffController(StaffService staffService)
        {
            _staffService = staffService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _staffService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
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
            var nhanVien = await _staffService.GetByIdAsync(id);
            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var nhanVien = await _staffService.GetByIdAsync(id);
            if (nhanVien == null) return NotFound();

            return View(nhanVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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