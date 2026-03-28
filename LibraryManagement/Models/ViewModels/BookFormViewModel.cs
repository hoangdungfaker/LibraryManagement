using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Models.ViewModels
{
    public class BookFormViewModel
    {
        public int MaSach { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống.")]
        public string TenSach { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tác giả không được để trống.")]
        public string TacGia { get; set; } = string.Empty;

        public string? TheLoai { get; set; }
        public string? NhaXuatBan { get; set; }
        public int? NamXuatBan { get; set; }
        public string? ViTri { get; set; }

        [Display(Name = "Tên ảnh thủ công")]
        public string? TenHinhAnh { get; set; }

        [Display(Name = "Chọn file ảnh")]
        public IFormFile? FileHinhAnh { get; set; }

        public bool CoAnhHienTai { get; set; }
    }
}