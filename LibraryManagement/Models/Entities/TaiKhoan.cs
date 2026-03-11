using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("TAI_KHOAN")]
    public class TaiKhoan
    {
        [Key]
        [Column("ma_tai_khoan")]
        public int MaTaiKhoan { get; set; }

        [Required]
        [StringLength(100)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("mat_khau")]
        public string MatKhau { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("ho_ten")]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("trang_thai")]
        public string TrangThai { get; set; } = "HoatDong";

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; }

        public virtual DocGia? DocGia { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
        public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
    }
}