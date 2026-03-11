using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.ViewModels
{
    public class NotificationCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn tài khoản.")]
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống.")]
        [StringLength(500)]
        public string NoiDung { get; set; } = string.Empty;

        public List<TaiKhoan> DanhSachTaiKhoan { get; set; } = new();
    }
}