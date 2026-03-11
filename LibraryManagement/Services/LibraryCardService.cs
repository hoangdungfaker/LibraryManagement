using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class LibraryCardService
    {
        private readonly LibraryDbContext _context;

        public LibraryCardService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<TheThuVien>> GetAllAsync()
        {
            return await _context.TheThuViens
                .Include(x => x.DocGia)
                .ThenInclude(x => x.TaiKhoan)
                .OrderByDescending(x => x.NgayCap)
                .ToListAsync();
        }

        public async Task<TheThuVien?> GetByIdAsync(int id)
        {
            return await _context.TheThuViens
                .Include(x => x.DocGia)
                .ThenInclude(x => x.TaiKhoan)
                .FirstOrDefaultAsync(x => x.MaThe == id);
        }

        public async Task AddAsync(TheThuVien card)
        {
            _context.TheThuViens.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TheThuVien card)
        {
            _context.TheThuViens.Update(card);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DocGia>> GetDocGiaChuaCoTheAsync()
        {
            var docGiaDaCoThe = await _context.TheThuViens
                .Select(x => x.MaDocGia)
                .ToListAsync();

            return await _context.DocGias
                .Include(x => x.TaiKhoan)
                .Where(x => !docGiaDaCoThe.Contains(x.MaDocGia))
                .ToListAsync();
        }
    }
}