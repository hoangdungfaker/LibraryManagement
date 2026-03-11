using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.ViewModels
{
    public class ReaderFormViewModel
    {
        public int MaDocGia { get; set; }

        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [StringLength(20)]
        public string MaSinhVien { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn tài khoản.")]
        public int MaTaiKhoan { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }

        public List<TaiKhoan> DanhSachTaiKhoan { get; set; } = new();
    }
}   