using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("PHIEU_MUON")]
    public class PhieuMuon
    {
        [Key]
        [Column("ma_phieu")]
        public int MaPhieu { get; set; }

        [Column("ma_the")]
        public int MaThe { get; set; }

        [Column("ma_nhan_vien")]
        public int MaNhanVien { get; set; }

        [Column("ngay_muon")]
        public DateTime NgayMuon { get; set; }

        [Column("han_tra")]
        public DateTime HanTra { get; set; }

        [Column("so_lan_gia_han")]
        public int SoLanGiaHan { get; set; }

        [Required]
        [StringLength(20)]
        [Column("trang_thai")]
        public string TrangThai { get; set; } = "DangMuon";

        public virtual TheThuVien TheThuVien { get; set; } = null!;
        public virtual NhanVien NhanVien { get; set; } = null!;
        public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
        public virtual ICollection<TienPhat> TienPhats { get; set; } = new List<TienPhat>();
    }
}