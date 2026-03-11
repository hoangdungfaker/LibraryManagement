using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("CHI_TIET_PHIEU_MUON")]
    public class ChiTietPhieuMuon
    {
        [Column("ma_phieu")]
        public int MaPhieu { get; set; }

        [Column("ma_cuon")]
        public int MaCuon { get; set; }

        [Column("ngay_tra")]
        public DateTime? NgayTra { get; set; }

        [StringLength(20)]
        [Column("tinh_trang")]
        public string? TinhTrang { get; set; }

        public virtual PhieuMuon PhieuMuon { get; set; } = null!;
        public virtual CuonSach CuonSach { get; set; } = null!;
    }
}