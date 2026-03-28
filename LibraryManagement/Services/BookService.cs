using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace LibraryManagement.Services
{
    public class BookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public async Task<List<Sach>> GetAllAsync()
        {
            return await _context.Sachs
                .OrderBy(x => x.TenSach)
                .ToListAsync();
        }

        // tìm kiếm sách 
        public async Task<List<Sach>> SearchAsync(string? keyword, string? category)
        {
            var allBooks = await _context.Sachs.ToListAsync();
            var result = allBooks.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = RemoveDiacritics(keyword.Trim()).ToLower();
                var keywords = normalizedKeyword.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                result = result.Where(x =>
                {
                    if (x.TenSach == null) return false;

                    var normalizedTitle = RemoveDiacritics(x.TenSach).ToLower();

                    return keywords.Any(kw => normalizedTitle.Contains(kw));
                });
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var lowerCategory = category.Trim().ToLower();
                result = result.Where(x => x.TheLoai != null && x.TheLoai.ToLower() == lowerCategory);
            }

            return result
                .OrderBy(x => x.TenSach)
                .ToList();
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