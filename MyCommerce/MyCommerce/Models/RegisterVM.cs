using System.ComponentModel.DataAnnotations;

namespace MyCommerce.Models
{
    public class RegisterVM
    {
        [Key]
        public string MaKh { get; set; }

        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp")]
        public string MatKhauNhapLai { get; set; }

        public string HoTen { get; set; }
        public bool GioiTinh { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string Email { get; set; }
        public string? Hinh { get; set; }
    }

}
