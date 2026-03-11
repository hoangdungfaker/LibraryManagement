using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("DOC_GIA")]
    public class DocGia
    {
        [Key]
        [Column("ma_doc_gia")]
        public int MaDocGia { get; set; }

        [Required]
        [StringLength(20)]
        [Column("ma_sinh_vien")]
        public string MaSinhVien { get; set; } = string.Empty;

        [Column("ma_tai_khoan")]
        public int MaTaiKhoan { get; set; }

        [Column("ngay_sinh")]
        public DateTime? NgaySinh { get; set; }

        [StringLength(255)]
        [Column("dia_chi")]
        public string? DiaChi { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; } = null!;
        public virtual ICollection<TheThuVien> TheThuViens { get; set; } = new List<TheThuVien>();
    }
}