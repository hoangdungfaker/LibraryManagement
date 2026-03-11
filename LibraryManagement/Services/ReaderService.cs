using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class ReaderService
    {
        private readonly LibraryDbContext _context;

        public ReaderService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocGia>> GetAllAsync()
        {
            return await _context.DocGias
                .Include(x => x.TaiKhoan)
                .OrderBy(x => x.MaDocGia)
                .ToListAsync();
        }

        public async Task<DocGia?> GetByIdAsync(int id)
        {
            return await _context.DocGias
                .Include(x => x.TaiKhoan)
                .Include(x => x.TheThuViens)
                .FirstOrDefaultAsync(x => x.MaDocGia == id);
        }

        public async Task<bool> IsMaSinhVienExistsAsync(string maSinhVien, int? ignoreId = null)
        {
            return await _context.DocGias.AnyAsync(x =>
                x.MaSinhVien == maSinhVien &&
                (!ignoreId.HasValue || x.MaDocGia != ignoreId.Value));
        }

        public async Task AddAsync(DocGia docGia)
        {
            _context.DocGias.Add(docGia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DocGia docGia)
        {
            _context.DocGias.Update(docGia);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var docGia = await _context.DocGias
                .Include(x => x.TheThuViens)
                .FirstOrDefaultAsync(x => x.MaDocGia == id);

            if (docGia == null) return false;

            if (docGia.TheThuViens.Any())
                return false;

            _context.DocGias.Remove(docGia);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaiKhoan>> GetTaiKhoanChuaGanDocGiaAsync()
        {
            var taiKhoanDaGan = await _context.DocGias
                .Select(x => x.MaTaiKhoan)
                .ToListAsync();

            return await _context.TaiKhoans
                .Where(x => !taiKhoanDaGan.Contains(x.MaTaiKhoan))
                .OrderBy(x => x.HoTen)
                .ToListAsync();
        }
    }
}