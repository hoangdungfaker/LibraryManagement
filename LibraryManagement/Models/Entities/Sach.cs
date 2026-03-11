using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("SACH")]
    public class Sach
    {
        [Key]
        [Column("ma_sach")]
        public int MaSach { get; set; }

        [Required]
        [StringLength(255)]
        [Column("ten_sach")]
        public string TenSach { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Column("tac_gia")]
        public string TacGia { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("the_loai")]
        public string? TheLoai { get; set; }

        [StringLength(150)]
        [Column("nha_xuat_ban")]
        public string? NhaXuatBan { get; set; }

        [Column("nam_xuat_ban")]
        public int? NamXuatBan { get; set; }

        [StringLength(100)]
        [Column("vi_tri")]
        public string? ViTri { get; set; }

        public virtual ICollection<CuonSach> CuonSachs { get; set; } = new List<CuonSach>();
        public virtual ICollection<DatCho> DatChos { get; set; } = new List<DatCho>();
    }
}