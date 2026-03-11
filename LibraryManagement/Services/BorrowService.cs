using LibraryManagement.Data;
using LibraryManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BorrowService
    {
        private readonly LibraryDbContext _context;

        public BorrowService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<PhieuMuon>> GetAllAsync()
        {
            return await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.NhanVien)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.ChiTietPhieuMuons)
                    .ThenInclude(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .OrderByDescending(x => x.NgayMuon)
                .ToListAsync();
        }

        public async Task<PhieuMuon?> GetByIdAsync(int id)
        {
            return await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.NhanVien)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.ChiTietPhieuMuons)
                    .ThenInclude(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .Include(x => x.TienPhats)
                .FirstOrDefaultAsync(x => x.MaPhieu == id);
        }

        public async Task<List<TheThuVien>> GetTheHopLeAsync()
        {
            var today = DateTime.Today;

            return await _context.TheThuViens
                .Include(x => x.DocGia)
                    .ThenInclude(x => x.TaiKhoan)
                .Where(x => x.TrangThai == "HoatDong" && x.NgayHetHan >= today)
                .OrderBy(x => x.MaThe)
                .ToListAsync();
        }

        public async Task<List<NhanVien>> GetNhanVienAsync()
        {
            return await _context.NhanViens
                .Include(x => x.TaiKhoan)
                .OrderBy(x => x.MaNhanVien)
                .ToListAsync();
        }

        public async Task<List<CuonSach>> GetCuonSachCoSanAsync()
        {
            return await _context.CuonSachs
                .Include(x => x.Sach)
                .Where(x => x.TrangThai == "CoSan")
                .OrderBy(x => x.MaCuon)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateBorrowAsync(
            int maThe,
            int maNhanVien,
            int maCuon,
            DateTime hanTra)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var the = await _context.TheThuViens
                    .Include(x => x.DocGia)
                    .FirstOrDefaultAsync(x => x.MaThe == maThe);

                if (the == null)
                    return (false, "Không tìm thấy thẻ thư viện.");

                if (the.TrangThai != "HoatDong")
                    return (false, "Thẻ thư viện đang bị khoá hoặc không hợp lệ.");

                if (the.NgayHetHan.Date < DateTime.Today)
                    return (false, "Thẻ thư viện đã hết hạn.");

                var nhanVien = await _context.NhanViens
                    .FirstOrDefaultAsync(x => x.MaNhanVien == maNhanVien);

                if (nhanVien == null)
                    return (false, "Không tìm thấy nhân viên.");

                var cuonSach = await _context.CuonSachs
                    .Include(x => x.Sach)
                    .FirstOrDefaultAsync(x => x.MaCuon == maCuon);

                if (cuonSach == null)
                    return (false, "Không tìm thấy cuốn sách.");

                if (cuonSach.TrangThai != "CoSan")
                    return (false, "Cuốn sách hiện không sẵn sàng để mượn.");

                var phieuMuon = new PhieuMuon
                {
                    MaThe = maThe,
                    MaNhanVien = maNhanVien,
                    NgayMuon = DateTime.Now,
                    HanTra = hanTra,
                    SoLanGiaHan = 0,
                    TrangThai = "DangMuon"
                };

                _context.PhieuMuons.Add(phieuMuon);
                await _context.SaveChangesAsync();

                var chiTiet = new ChiTietPhieuMuon
                {
                    MaPhieu = phieuMuon.MaPhieu,
                    MaCuon = maCuon,
                    NgayTra = null,
                    TinhTrang = null
                };

                _context.ChiTietPhieuMuons.Add(chiTiet);

                cuonSach.TrangThai = "DangMuon";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Lập phiếu mượn thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<List<PhieuMuon>> GetHistoryByDocGiaAsync(int maTaiKhoan)
        {
            return await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                .Include(x => x.ChiTietPhieuMuons)
                    .ThenInclude(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .Where(x => x.TheThuVien.DocGia.MaTaiKhoan == maTaiKhoan)
                .OrderByDescending(x => x.NgayMuon)
                .ToListAsync();
        }
        public async Task<List<PhieuMuon>> GetDangMuonAsync()
        {
            return await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.NhanVien)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.ChiTietPhieuMuons)
                    .ThenInclude(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .Where(x => x.TrangThai == "DangMuon" || x.TrangThai == "QuaHan")
                .OrderByDescending(x => x.NgayMuon)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> ReturnBookAsync(int maPhieu, int maCuon, string tinhTrang)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var phieu = await _context.PhieuMuons
                    .Include(x => x.ChiTietPhieuMuons)
                    .FirstOrDefaultAsync(x => x.MaPhieu == maPhieu);

                if (phieu == null)
                    return (false, "Không tìm thấy phiếu mượn.");

                var chiTiet = await _context.ChiTietPhieuMuons
                    .Include(x => x.CuonSach)
                    .FirstOrDefaultAsync(x => x.MaPhieu == maPhieu && x.MaCuon == maCuon);

                if (chiTiet == null)
                    return (false, "Không tìm thấy chi tiết phiếu mượn.");

                if (chiTiet.NgayTra != null)
                    return (false, "Cuốn sách này đã được trả trước đó.");

                chiTiet.NgayTra = DateTime.Now;
                chiTiet.TinhTrang = tinhTrang;

                if (tinhTrang == "BinhThuong")
                    chiTiet.CuonSach.TrangThai = "CoSan";
                else if (tinhTrang == "HuHong")
                    chiTiet.CuonSach.TrangThai = "HuHong";
                else if (tinhTrang == "Mat")
                    chiTiet.CuonSach.TrangThai = "Mat";

                decimal tongPhat = 0;
                var lyDoList = new List<string>();

                if (DateTime.Now.Date > phieu.HanTra.Date)
                {
                    var soNgayTre = (DateTime.Now.Date - phieu.HanTra.Date).Days;
                    var phatTre = soNgayTre * 5000m;
                    tongPhat += phatTre;
                    lyDoList.Add($"Trễ hạn {soNgayTre} ngày");
                }

                if (tinhTrang == "HuHong")
                {
                    tongPhat += 50000m;
                    lyDoList.Add("Hư hỏng sách");
                }
                else if (tinhTrang == "Mat")
                {
                    tongPhat += 100000m;
                    lyDoList.Add("Mất sách");
                }

                if (tongPhat > 0)
                {
                    var tienPhat = new TienPhat
                    {
                        MaPhieu = maPhieu,
                        SoTien = tongPhat,
                        LyDo = string.Join(", ", lyDoList),
                        DaThanhToan = false
                    };

                    _context.TienPhats.Add(tienPhat);
                }

                var conSachChuaTra = await _context.ChiTietPhieuMuons
                    .AnyAsync(x => x.MaPhieu == maPhieu && x.NgayTra == null && x.MaCuon != maCuon);

                phieu.TrangThai = conSachChuaTra ? "DangMuon" : "DaTra";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                if (tongPhat > 0)
                    return (true, $"Trả sách thành công. Phát sinh tiền phạt: {tongPhat:N0} VNĐ.");

                return (true, "Trả sách thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<List<TienPhat>> GetTienPhatByPhieuAsync(int maPhieu)
        {
            return await _context.TienPhats
                .Where(x => x.MaPhieu == maPhieu)
                .OrderByDescending(x => x.MaPhat)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> MarkOverdueAsync()
        {
            try
            {
                var ds = await _context.PhieuMuons
                    .Where(x => x.TrangThai == "DangMuon" && x.HanTra.Date < DateTime.Today)
                    .ToListAsync();

                foreach (var item in ds)
                {
                    item.TrangThai = "QuaHan";
                }

                await _context.SaveChangesAsync();
                return (true, $"Đã cập nhật {ds.Count} phiếu quá hạn.");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật quá hạn: {ex.Message}");
            }
        }
        public async Task<List<PhieuMuon>> GetCoTheGiaHanAsync()
        {
            var today = DateTime.Today;

            return await _context.PhieuMuons
                .Include(x => x.TheThuVien)
                    .ThenInclude(x => x.DocGia)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.NhanVien)
                    .ThenInclude(x => x.TaiKhoan)
                .Include(x => x.ChiTietPhieuMuons)
                    .ThenInclude(x => x.CuonSach)
                    .ThenInclude(x => x.Sach)
                .Where(x => x.TrangThai == "DangMuon" && x.HanTra.Date >= today)
                .OrderBy(x => x.HanTra)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> RenewBorrowAsync(int maPhieu, DateTime hanTraMoi)
        {
            try
            {
                var phieu = await _context.PhieuMuons
                    .Include(x => x.ChiTietPhieuMuons)
                    .FirstOrDefaultAsync(x => x.MaPhieu == maPhieu);

                if (phieu == null)
                    return (false, "Không tìm thấy phiếu mượn.");

                if (phieu.TrangThai != "DangMuon")
                    return (false, "Chỉ phiếu đang mượn mới được gia hạn.");

                if (phieu.HanTra.Date < DateTime.Today)
                    return (false, "Phiếu đã quá hạn, không thể gia hạn.");

                if (phieu.SoLanGiaHan >= 2)
                    return (false, "Phiếu đã đạt tối đa số lần gia hạn.");

                var conSachChuaTra = phieu.ChiTietPhieuMuons.Any(x => x.NgayTra == null);
                if (!conSachChuaTra)
                    return (false, "Phiếu đã hoàn tất, không thể gia hạn.");

                if (hanTraMoi.Date <= phieu.HanTra.Date)
                    return (false, "Hạn trả mới phải lớn hơn hạn trả hiện tại.");

                phieu.HanTra = hanTraMoi;
                phieu.SoLanGiaHan += 1;

                await _context.SaveChangesAsync();

                return (true, "Gia hạn phiếu mượn thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }

}