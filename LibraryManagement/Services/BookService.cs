using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sach>> GetAllAsync()
        {
            return await _context.Sachs
                .OrderBy(x => x.TenSach)
                .ToListAsync();
        }

        public async Task<List<Sach>> SearchAsync(string? keyword, string? category)
        {
            var query = _context.Sachs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.Trim().ToLower();
                query = query.Where(x => x.TenSach != null && x.TenSach.ToLower().Contains(lowerKeyword));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var lowerCategory = category.Trim().ToLower();
                query = query.Where(x => x.TheLoai != null && x.TheLoai.ToLower() == lowerCategory);
            }

            return await query
                .OrderBy(x => x.TenSach)
                .ToListAsync();
        }

        public async Task<Sach?> GetByIdAsync(int id)
        {
            return await _context.Sachs.FirstOrDefaultAsync(x => x.MaSach == id);
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Sachs
                .Where(x => x.TheLoai != null && x.TheLoai != "")
                .Select(x => x.TheLoai!)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }

        public async Task AddAsync(Sach sach)
        {
            _context.Sachs.Add(sach);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Sach sach)
        {
            _context.Sachs.Update(sach);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sach = await _context.Sachs.FirstOrDefaultAsync(x => x.MaSach == id);
            if (sach == null) return false;

            var hasCopies = await _context.CuonSachs.AnyAsync(x => x.MaSach == id);
            if (hasCopies) return false;

            _context.Sachs.Remove(sach);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}