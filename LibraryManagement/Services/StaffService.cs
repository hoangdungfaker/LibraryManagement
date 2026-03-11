using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class StaffService
    {
        private readonly LibraryDbContext _context;

        public StaffService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<NhanVien>> GetAllAsync()
        {
            return await _context.NhanViens
                .Include(x => x.TaiKhoan)
                .OrderBy(x => x.MaNhanVien)
                .ToListAsync();
        }

        public async Task<NhanVien?> GetByIdAsync(int id)
        {
            return await _context.NhanViens
                .Include(x => x.TaiKhoan)
                .Include(x => x.PhieuMuons)
                .FirstOrDefaultAsync(x => x.MaNhanVien == id);
        }

        public async Task AddAsync(NhanVien nhanVien)
        {
            _context.NhanViens.Add(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVien nhanVien)
        {
            _context.NhanViens.Update(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var nhanVien = await _context.NhanViens
                .Include(x => x.PhieuMuons)
                .FirstOrDefaultAsync(x => x.MaNhanVien == id);

            if (nhanVien == null) return false;

            if (nhanVien.PhieuMuons.Any())
                return false;

            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaiKhoan>> GetTaiKhoanChuaGanNhanVienAsync()
        {
            var taiKhoanDaGan = await _context.NhanViens
                .Select(x => x.MaTaiKhoan)
                .ToListAsync();

            return await _context.TaiKhoans
                .Where(x => !taiKhoanDaGan.Contains(x.MaTaiKhoan))
                .OrderBy(x => x.HoTen)
                .ToListAsync();
        }
    }
}