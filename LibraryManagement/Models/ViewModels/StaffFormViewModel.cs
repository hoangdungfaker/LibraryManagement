using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.ViewModels
{
    public class StaffFormViewModel
    {
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tài khoản.")]
        public int MaTaiKhoan { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Chức vụ không được để trống.")]
        public string ChucVu { get; set; } = string.Empty;

        public List<TaiKhoan> DanhSachTaiKhoan { get; set; } = new();
    }
}