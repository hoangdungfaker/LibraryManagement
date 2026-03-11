using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("VAI_TRO")]
    public class VaiTro
    {
        [Key]
        [Column("ma_vai_tro")]
        public int MaVaiTro { get; set; }

        [Required]
        [StringLength(50)]
        [Column("ten_vai_tro")]
        public string TenVaiTro { get; set; } = string.Empty;

        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
    }
}