using LibraryManagement.Data;
using LibraryManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class ReportService
    {
        private readonly LibraryDbContext _context;

        public ReportService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetTongQuanAsync()
        {
            var model = new ReportViewModel
            {
                TongSoSach = await _context.Sachs.CountAsync(),
                TongSoCuonSach = await _context.CuonSachs.CountAsync(),
                TongSoDocGia = await _context.DocGias.CountAsync(),
                TongSoNhanVien = await _context.NhanViens.CountAsync(),

                TongPhieuDangMuon = await _context.PhieuMuons
                    .CountAsync(x => x.TrangThai == "DangMuon"),

                TongPhieuDaTra = await _context.PhieuMuons
                    .CountAsync(x => x.TrangThai == "DaTra"),

                TongPhieuQuaHan = await _context.PhieuMuons
                    .CountAsync(x => x.TrangThai == "QuaHan"),

                TongTienPhatChuaThanhToan = await _context.TienPhats
                    .Where(x => !x.DaThanhToan)
                    .SumAsync(x => (decimal?)x.SoTien) ?? 0
            };

            model.TopSachMuonNhieu = await _context.ChiTietPhieuMuons
                .Include(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .GroupBy(x => new
                {
                    x.CuonSach.MaSach,
                    x.CuonSach.Sach.TenSach,
                    x.CuonSach.Sach.TacGia
                })
                .Select(g => new TopSachMuonNhieuViewModel
                {
                    MaSach = g.Key.MaSach,
                    TenSach = g.Key.TenSach,
                    TacGia = g.Key.TacGia,
                    SoLuotMuon = g.Count()
                })
                .OrderByDescending(x => x.SoLuotMuon)
                .Take(10)
                .ToListAsync();

            return model;
        }
    }
}