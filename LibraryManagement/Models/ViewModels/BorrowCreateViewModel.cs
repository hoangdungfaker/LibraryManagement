using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.ViewModels
{
    public class BorrowCreateViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn thẻ thư viện.")]
        public int MaThe { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhân viên.")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn cuốn sách.")]
        public int MaCuon { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hạn trả.")]
        [DataType(DataType.Date)]
        public DateTime HanTra { get; set; }

        public List<TheThuVien> DanhSachThe { get; set; } = new();
        public List<NhanVien> DanhSachNhanVien { get; set; } = new();
        public List<CuonSach> DanhSachCuonSach { get; set; } = new();
    }
}