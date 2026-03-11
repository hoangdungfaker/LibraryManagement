using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("CUON_SACH")]
    public class CuonSach
    {
        [Key]
        [Column("ma_cuon")]
        public int MaCuon { get; set; }

        [Column("ma_sach")]
        public int MaSach { get; set; }

        [Required]
        [StringLength(20)]
        [Column("trang_thai")]
        public string TrangThai { get; set; } = "CoSan";

        public virtual Sach Sach { get; set; } = null!;
        public virtual ICollection<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; } = new List<ChiTietPhieuMuon>();
    }
}