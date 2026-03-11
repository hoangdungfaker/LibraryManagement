using LibraryManagement.Models.Entities;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        private bool KiemTraTruyCap()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan");
            var vaiTro = HttpContext.Session.GetString("VaiTro");

            return maTaiKhoan != null && (vaiTro == "Admin" || vaiTro == "ThuThu");
        }

        public async Task<IActionResult> Index()
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var dsSach = await _bookService.GetAllAsync();
            return View(dsSach);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sach sach)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
                return View(sach);

            await _bookService.AddAsync(sach);
            TempData["Success"] = "Thêm sách thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Sach sach)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

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
        public async Task<IActionResult> Delete(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

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
            if (HttpContext.Session.GetInt32("MaTaiKhoan") == null)
                return RedirectToAction("Login", "Account");

            var sach = await _bookService.GetByIdAsync(id);
            if (sach == null)
                return NotFound();

            return View(sach);
        }
    }
}