using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models.ViewModels
{
    public class BorrowBookViewModel
    {
        [Required(ErrorMessage = "Mã thẻ không được để trống.")]
        public int MaThe { get; set; }

        [Required(ErrorMessage = "Mã nhân viên không được để trống.")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Mã cuốn sách không được để trống.")]
        public int MaCuon { get; set; }
    }
}