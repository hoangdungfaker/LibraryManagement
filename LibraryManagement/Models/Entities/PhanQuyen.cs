using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entities
{
    [Table("PHAN_QUYEN")]
    public class PhanQuyen
    {
        [Column("ma_tai_khoan")]
        public int MaTaiKhoan { get; set; }

        [Column("ma_vai_tro")]
        public int MaVaiTro { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; } = null!;
        public virtual VaiTro VaiTro { get; set; } = null!;
    }
}