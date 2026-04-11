using LibraryManagement.Models.ViewModels;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Filters;

namespace LibraryManagement.Controllers
{
    [KiemTraQuyen]
    public class BorrowController : Controller
    {
        private readonly BorrowService _borrowService;

        public BorrowController(BorrowService borrowService)
        {
            _borrowService = borrowService;
        }
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Index()
        {
            var data = await _borrowService.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Create()
        {
            var vm = new BorrowCreateViewModel
            {
                HanTra = DateTime.Today.AddDays(14),
                DanhSachThe = await _borrowService.GetTheHopLeAsync(),
                DanhSachNhanVien = await _borrowService.GetNhanVienAsync(),
                DanhSachCuonSach = await _borrowService.GetCuonSachCoSanAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Create(BorrowCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachThe = await _borrowService.GetTheHopLeAsync();
                model.DanhSachNhanVien = await _borrowService.GetNhanVienAsync();
                model.DanhSachCuonSach = await _borrowService.GetCuonSachCoSanAsync();
                return View(model);
            }

            var result = await _borrowService.CreateBorrowAsync(
                model.MaThe,
                model.MaNhanVien,
                model.MaCuon,
                model.HanTra);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                model.DanhSachThe = await _borrowService.GetTheHopLeAsync();
                model.DanhSachNhanVien = await _borrowService.GetNhanVienAsync();
                model.DanhSachCuonSach = await _borrowService.GetCuonSachCoSanAsync();
                return View(model);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var phieu = await _borrowService.GetByIdAsync(id);
            if (phieu == null) return NotFound();
            var role = HttpContext.Session.GetString("VaiTro");
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;

            if (role == "DocGia" && phieu.TheThuVien.DocGia.MaTaiKhoan != maTaiKhoan)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            return View(phieu);
        }

        [KiemTraQuyen("DocGia")]
        public async Task<IActionResult> History()
        {
            var maTaiKhoan = HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;
            var data = await _borrowService.GetHistoryByDocGiaAsync(maTaiKhoan);
            return View(data);
        }
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> ReturnList()
        {
            await _borrowService.MarkOverdueAsync();
            var data = await _borrowService.GetDangMuonAsync();
            return View(data);
        }

        [HttpGet]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Return(int maPhieu, int maCuon)
        {
            var phieu = await _borrowService.GetByIdAsync(maPhieu);
            if (phieu == null) return NotFound();

            var chiTiet = phieu.ChiTietPhieuMuons.FirstOrDefault(x => x.MaCuon == maCuon);
            if (chiTiet == null) return NotFound();

            var vm = new LibraryManagement.Models.ViewModels.ReturnBookViewModel
            {
                MaPhieu = maPhieu,
                MaCuon = maCuon,
                TinhTrang = "BinhThuong"
            };

            ViewBag.Phieu = phieu;
            ViewBag.ChiTiet = chiTiet;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Return(LibraryManagement.Models.ViewModels.ReturnBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var phieuInvalid = await _borrowService.GetByIdAsync(model.MaPhieu);
                ViewBag.Phieu = phieuInvalid;
                ViewBag.ChiTiet = phieuInvalid?.ChiTietPhieuMuons.FirstOrDefault(x => x.MaCuon == model.MaCuon);
                return View(model);
            }

            var result = await _borrowService.ReturnBookAsync(model.MaPhieu, model.MaCuon, model.TinhTrang);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                var phieuInvalid = await _borrowService.GetByIdAsync(model.MaPhieu);
                ViewBag.Phieu = phieuInvalid;
                ViewBag.ChiTiet = phieuInvalid?.ChiTietPhieuMuons.FirstOrDefault(x => x.MaCuon == model.MaCuon);
                return View(model);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(ReturnList));
        }

        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> RenewList()
        {
            var data = await _borrowService.GetCoTheGiaHanAsync();
            return View(data);
        }

        [HttpGet]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Renew(int id)
        {
            var phieu = await _borrowService.GetByIdAsync(id);
            if (phieu == null) return NotFound();

            var vm = new LibraryManagement.Models.ViewModels.RenewBorrowViewModel
            {
                MaPhieu = phieu.MaPhieu,
                HanTraCu = phieu.HanTra,
                HanTraMoi = phieu.HanTra.AddDays(7),
                SoLanGiaHan = phieu.SoLanGiaHan
            };

            ViewBag.Phieu = phieu;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [KiemTraQuyen("Admin", "ThuThu")]
        public async Task<IActionResult> Renew(LibraryManagement.Models.ViewModels.RenewBorrowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Phieu = await _borrowService.GetByIdAsync(model.MaPhieu);
                return View(model);
            }

            var result = await _borrowService.RenewBorrowAsync(model.MaPhieu, model.HanTraMoi);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Phieu = await _borrowService.GetByIdAsync(model.MaPhieu);
                return View(model);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction(nameof(RenewList));
        }
    }
}