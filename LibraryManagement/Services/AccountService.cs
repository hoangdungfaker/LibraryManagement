using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class AccountService
    {
        private readonly LibraryDbContext _context;

        public AccountService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<TaiKhoan?> LoginAsync(string email, string matKhau)
        {
            var taiKhoan = await _context.TaiKhoans
                .Include(t => t.PhanQuyens)
                    .ThenInclude(pq => pq.VaiTro)
                .FirstOrDefaultAsync(t =>
                    t.Email == email &&
                    t.MatKhau == matKhau &&
                    t.TrangThai == "HoatDong");

            return taiKhoan;
        }

        public async Task<string?> GetRoleAsync(int maTaiKhoan)
        {
            var role = await _context.PhanQuyens
                .Where(pq => pq.MaTaiKhoan == maTaiKhoan)
                .Include(pq => pq.VaiTro)
                .Select(pq => pq.VaiTro.TenVaiTro)
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<TaiKhoan?> GetByIdAsync(int maTaiKhoan)
        {
            return await _context.TaiKhoans
                .Include(t => t.PhanQuyens)
                    .ThenInclude(pq => pq.VaiTro)
                .FirstOrDefaultAsync(t => t.MaTaiKhoan == maTaiKhoan);
        }
    }
}