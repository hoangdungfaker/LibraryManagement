using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("THONG_BAO")]
    public class ThongBao
    {
        [Key]
        [Column("ma_thong_bao")]
        public int MaThongBao { get; set; }

        [Column("ma_tai_khoan")]
        public int MaTaiKhoan { get; set; }

        [Required]
        [StringLength(500)]
        [Column("noi_dung")]
        public string NoiDung { get; set; } = string.Empty;

        [Column("ngay_gui")]
        public DateTime NgayGui { get; set; }

        [Column("da_doc")]
        public bool DaDoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; } = null!;
    }
}