using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models.Entities;

namespace LibraryManagement.Models.ViewModels
{
    public class LibraryCardFormViewModel
    {
        public int MaThe { get; set; }

        [Required]
        public int MaDocGia { get; set; }

        [Required]
        public DateTime NgayCap { get; set; }

        [Required]
        public DateTime NgayHetHan { get; set; }

        public string TrangThai { get; set; } = "HoatDong";

        public List<DocGia> DanhSachDocGia { get; set; } = new();
    }
}