using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;
using System.Security.Claims;

namespace MyCommerce.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly MyeStoreContext _ctx;

        public KhachHangController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile FileHinh)
        {
            try
            {
                var khachHang = new KhachHang
                {
                    MaKh = model.MaKh,
                    HoTen = model.HoTen,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    GioiTinh = model.GioiTinh,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    Hinh = MyTool.UploadFileToFolder(FileHinh, "KhachHang"),

                    HieuLuc = true, //false + gửi mail active tài khoản
                    RandomKey = MyTool.GetRandom()
                };
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                _ctx.Add(khachHang);
                _ctx.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            catch (Exception ex)
            {
                return View();
            }


        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model)
        {
            var kh = _ctx.KhachHangs.SingleOrDefault(p => p.MaKh == model.UserName);
            if (kh == null)
            {
                ViewBag.ThongBaoLoi = "User này không tồn tại";
                return View();
            }

            if (kh.MatKhau != model.Password.ToMd5Hash(kh.RandomKey))
            {
                ViewBag.ThongBaoLoi = "Đăng nhập không thành công";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.Email),
                new Claim("FullName", kh.HoTen),

                new Claim(ClaimTypes.Role, "Administrator"),//động
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimPrincipal);

            return Redirect("/");
        }

    }

}
