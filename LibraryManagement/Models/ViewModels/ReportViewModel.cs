namespace LibraryManagement.Models.ViewModels
{
    public class ReportViewModel
    {
        public int TongSoSach { get; set; }
        public int TongSoCuonSach { get; set; }
        public int TongSoDocGia { get; set; }
        public int TongSoNhanVien { get; set; }

        public int TongPhieuDangMuon { get; set; }
        public int TongPhieuDaTra { get; set; }
        public int TongPhieuQuaHan { get; set; }

        public decimal TongTienPhatChuaThanhToan { get; set; }

        public List<TopSachMuonNhieuViewModel> TopSachMuonNhieu { get; set; } = new();
    }

    public class TopSachMuonNhieuViewModel
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; } = string.Empty;
        public string TacGia { get; set; } = string.Empty;
        public int SoLuotMuon { get; set; }
    }
}