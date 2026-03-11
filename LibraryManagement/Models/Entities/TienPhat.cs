using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("TIEN_PHAT")]
    public class TienPhat
    {
        [Key]
        [Column("ma_phat")]
        public int MaPhat { get; set; }

        [Column("ma_phieu")]
        public int MaPhieu { get; set; }

        [Column("so_tien", TypeName = "decimal(18,2)")]
        public decimal SoTien { get; set; }

        [Required]
        [StringLength(255)]
        [Column("ly_do")]
        public string LyDo { get; set; } = string.Empty;

        [Column("da_thanh_toan")]
        public bool DaThanhToan { get; set; }

        public virtual PhieuMuon PhieuMuon { get; set; } = null!;
    }
}