using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("DAT_CHO")]
    public class DatCho
    {
        [Key]
        [Column("ma_dat_cho")]
        public int MaDatCho { get; set; }

        [Column("ma_the")]
        public int MaThe { get; set; }

        [Column("ma_sach")]
        public int MaSach { get; set; }

        [Column("ngay_dat")]
        public DateTime NgayDat { get; set; }

        [Required]
        [StringLength(20)]
        [Column("trang_thai")]
        public string TrangThai { get; set; } = "ChoXuLy";

        public virtual TheThuVien TheThuVien { get; set; } = null!;
        public virtual Sach Sach { get; set; } = null!;
    }
}