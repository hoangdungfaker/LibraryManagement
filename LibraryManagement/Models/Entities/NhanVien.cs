using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("NHAN_VIEN")]
    public class NhanVien
    {
        [Key]
        [Column("ma_nhan_vien")]
        public int MaNhanVien { get; set; }

        [Column("ma_tai_khoan")]
        public int MaTaiKhoan { get; set; }

        [StringLength(100)]
        [Column("chuc_vu")]
        public string? ChucVu { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; } = null!;
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
    }
}