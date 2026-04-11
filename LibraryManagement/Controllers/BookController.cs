using LibraryManagement.Models.Entities;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen]
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<IActionResult> Index(string? keyword, string? category)
        {
            var dsSach = await _bookService.SearchAsync(keyword, category);
            var categories = await _bookService.GetCategoriesAsync();

            ViewBag.Keyword = keyword;
            ViewBag.Category = category;
            ViewBag.Categories = categories;

            return View(dsSach);
        }

        [HttpGet]
        [KiemTraQuyen("Admin","ThuThu")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Create(Sach sach)
        {
            if (!ModelState.IsValid)
                return View(sach);

            await _bookService.AddAsync(sach);
            TempData["Success"] = "Thêm sách thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Edit(int id)
        {
            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Edit(Sach sach)
        {
            if (!ModelState.IsValid)
                return View(sach);

            var sachDb = await _bookService.GetByIdAsync(sach.MaSach);
            if (sachDb == null)
                return NotFound();

            sachDb.TenSach = sach.TenSach;
            sachDb.TacGia = sach.TacGia;
            sachDb.TheLoai = sach.TheLoai;
            sachDb.NhaXuatBan = sach.NhaXuatBan;
            sachDb.NamXuatBan = sach.NamXuatBan;
            sachDb.ViTri = sach.ViTri;

            await _bookService.UpdateAsync(sachDb);
            TempData["Success"] = "Cập nhật sách thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Delete(int id)
        {
            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Không thể xoá sách vì sách đã có cuốn sách liên quan.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Xoá sách thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }
    }
}