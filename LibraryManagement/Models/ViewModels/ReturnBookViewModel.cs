using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models.ViewModels
{
    public class ReturnBookViewModel
    {
        [Required(ErrorMessage = "Mã phiếu không được để trống.")]
        public int MaPhieu { get; set; }

        [Required(ErrorMessage = "Mã cuốn sách không được để trống.")]
        public int MaCuon { get; set; }

        [Required(ErrorMessage = "Tình trạng sách không được để trống.")]
        public string TinhTrang { get; set; } = "BinhThuong";
    }
}