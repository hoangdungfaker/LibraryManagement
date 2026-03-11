using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models.ViewModels
{
    public class RenewBorrowViewModel
    {
        [Required]
        public int MaPhieu { get; set; }

        public DateTime HanTraCu { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hạn trả mới.")]
        [DataType(DataType.Date)]
        public DateTime HanTraMoi { get; set; }

        public int SoLanGiaHan { get; set; }
    }
}