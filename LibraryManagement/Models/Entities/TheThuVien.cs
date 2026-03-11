using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("THE_THU_VIEN")]
    public class TheThuVien
    {
        [Key]
        [Column("ma_the")]
        public int MaThe { get; set; }

        [Column("ma_doc_gia")]
        public int MaDocGia { get; set; }

        [Column("ngay_cap")]
        public DateTime NgayCap { get; set; }

        [Column("ngay_het_han")]
        public DateTime NgayHetHan { get; set; }

        [Required]
        [StringLength(20)]
        [Column("trang_thai")]
        public string TrangThai { get; set; } = "HoatDong";

        public virtual DocGia DocGia { get; set; } = null!;
        public virtual ICollection<DatCho> DatChos { get; set; } = new List<DatCho>();
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; } = new List<PhieuMuon>();
    }
}