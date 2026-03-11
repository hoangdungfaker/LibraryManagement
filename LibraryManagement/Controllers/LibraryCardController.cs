using LibraryManagement.Models.Entities;
using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class LibraryCardController : Controller
    {
        private readonly LibraryCardService _cardService;

        public LibraryCardController(LibraryCardService cardService)
        {
            _cardService = cardService;
        }

        private bool KiemTraTruyCap()
        {
            var role = HttpContext.Session.GetString("VaiTro");
            return role == "Admin" || role == "ThuThu";
        }

        public async Task<IActionResult> Index()
        {
            if (!KiemTraTruyCap())
                return RedirectToAction("AccessDenied", "Account");

            var data = await _cardService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new LibraryCardFormViewModel
            {
                NgayCap = DateTime.Now,
                NgayHetHan = DateTime.Now.AddYears(1),
                DanhSachDocGia = await _cardService.GetDocGiaChuaCoTheAsync()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LibraryCardFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachDocGia = await _cardService.GetDocGiaChuaCoTheAsync();
                return View(model);
            }

            var card = new TheThuVien
            {
                MaDocGia = model.MaDocGia,
                NgayCap = model.NgayCap,
                NgayHetHan = model.NgayHetHan,
                TrangThai = "HoatDong"
            };

            await _cardService.AddAsync(card);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card == null) return NotFound();

            return View(card);
        }

        public async Task<IActionResult> Extend(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card == null) return NotFound();

            card.NgayHetHan = card.NgayHetHan.AddYears(1);
            await _cardService.UpdateAsync(card);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Lock(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card == null) return NotFound();

            card.TrangThai = "BiKhoa";
            await _cardService.UpdateAsync(card);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Unlock(int id)
        {
            var card = await _cardService.GetByIdAsync(id);
            if (card == null) return NotFound();

            card.TrangThai = "HoatDong";
            await _cardService.UpdateAsync(card);

            return RedirectToAction(nameof(Index));
        }
    }
}