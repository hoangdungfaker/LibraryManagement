using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using LibraryManagement.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class NotificationService
    {
        private readonly LibraryDbContext _context;

        public NotificationService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<ThongBao>> GetAllAsync()
        {
            return await _context.ThongBaos
                .Include(x => x.TaiKhoan)
                .OrderByDescending(x => x.NgayGui)
                .ToListAsync();
        }

        public async Task<List<ThongBao>> GetByTaiKhoanAsync(int maTaiKhoan)
        {
            return await _context.ThongBaos
                .Where(x => x.MaTaiKhoan == maTaiKhoan)
                .OrderByDescending(x => x.NgayGui)
                .ToListAsync();
        }

        public async Task<ThongBao?> GetByIdAsync(int id)
        {
            return await _context.ThongBaos
                .Include(x => x.TaiKhoan)
                .FirstOrDefaultAsync(x => x.MaThongBao == id);
        }

        public async Task<List<TaiKhoan>> GetAllTaiKhoanAsync()
        {
            return await _context.TaiKhoans
                .OrderBy(x => x.HoTen)
                .ToListAsync();
        }

        public async Task CreateAsync(int maTaiKhoan, string noiDung)
        {
            var thongBao = new ThongBao
            {
                MaTaiKhoan = maTaiKhoan,
                NoiDung = noiDung,
                NgayGui = DateTime.Now,
                DaDoc = false
            };

            _context.ThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var thongBao = await _context.ThongBaos.FirstOrDefaultAsync(x => x.MaThongBao == id);
            if (thongBao == null) return;

            thongBao.DaDoc = true;
            await _context.SaveChangesAsync();
        }

        public async Task<int> SendOverdueNotificationsAsync()
        {
            var today = DateTime.Today;

            var dsQuaHan = await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                        .ThenInclude(x => x.TaiKhoan)
                .Where(x => (x.TrangThai == "DangMuon" || x.TrangThai == "QuaHan") && x.HanTra.Date < today)
                .ToListAsync();

            int count = 0;

            foreach (var phieu in dsQuaHan)
            {
                var maTaiKhoan = phieu.TheThuVien.DocGia.MaTaiKhoan;
                var noiDung = $"Phiếu mượn #{phieu.MaPhieu} của bạn đã quá hạn từ ngày {phieu.HanTra:dd/MM/yyyy}. Vui lòng trả sách sớm.";

                var thongBao = new ThongBao
                {
                    MaTaiKhoan = maTaiKhoan,
                    NoiDung = noiDung,
                    NgayGui = DateTime.Now,
                    DaDoc = false
                };

                _context.ThongBaos.Add(thongBao);
                count++;
            }

            await _context.SaveChangesAsync();
            return count;
        }
    }
}