using LibraryManagement.Models.Entities;
using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen("Admin", "ThuThu")]
    public class ReaderController : Controller
    {
        private readonly ReaderService _readerService;

        public ReaderController(ReaderService readerService)
        {
            _readerService = readerService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _readerService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new ReaderFormViewModel
            {
                DanhSachTaiKhoan = await _readerService.GetTaiKhoanChuaGanDocGiaAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReaderFormViewModel model)
        {
            if (await _readerService.IsMaSinhVienExistsAsync(model.MaSinhVien))
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                model.DanhSachTaiKhoan = await _readerService.GetTaiKhoanChuaGanDocGiaAsync();
                return View(model);
            }

            var docGia = new DocGia
            {
                MaSinhVien = model.MaSinhVien,
                MaTaiKhoan = model.MaTaiKhoan,
                NgaySinh = model.NgaySinh,
                DiaChi = model.DiaChi
            };

            await _readerService.AddAsync(docGia);
            TempData["Success"] = "Thêm độc giả thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var docGia = await _readerService.GetByIdAsync(id);
            if (docGia == null) return NotFound();

            var taiKhoan = await _readerService.GetTaiKhoanChuaGanDocGiaAsync();
            var tkHienTai = docGia.TaiKhoan;
            if (tkHienTai != null && !taiKhoan.Any(x => x.MaTaiKhoan == tkHienTai.MaTaiKhoan))
            {
                taiKhoan.Insert(0, tkHienTai);
            }

            var vm = new ReaderFormViewModel
            {
                MaDocGia = docGia.MaDocGia,
                MaSinhVien = docGia.MaSinhVien,
                MaTaiKhoan = docGia.MaTaiKhoan,
                NgaySinh = docGia.NgaySinh,
                DiaChi = docGia.DiaChi,
                DanhSachTaiKhoan = taiKhoan
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReaderFormViewModel model)
        {

            if (await _readerService.IsMaSinhVienExistsAsync(model.MaSinhVien, model.MaDocGia))
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                model.DanhSachTaiKhoan = await _readerService.GetTaiKhoanChuaGanDocGiaAsync();
                return View(model);
            }

            var docGia = await _readerService.GetByIdAsync(model.MaDocGia);
            if (docGia == null) return NotFound();

            docGia.MaSinhVien = model.MaSinhVien;
            docGia.MaTaiKhoan = model.MaTaiKhoan;
            docGia.NgaySinh = model.NgaySinh;
            docGia.DiaChi = model.DiaChi;

            await _readerService.UpdateAsync(docGia);
            TempData["Success"] = "Cập nhật độc giả thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var docGia = await _readerService.GetByIdAsync(id);
            if (docGia == null) return NotFound();

            return View(docGia);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var docGia = await _readerService.GetByIdAsync(id);
            if (docGia == null) return NotFound();

            return View(docGia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var result = await _readerService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Không thể xoá độc giả vì đã có thẻ thư viện liên quan.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Xoá độc giả thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}   