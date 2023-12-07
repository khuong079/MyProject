using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
    public class CartController : Controller
    {
        private MyeStoreContext _ctx;

        public CartController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }

        public List<CartItem> Cart
        {
            get
            {
                var carts = HttpContext.Session.Get<List<CartItem>>("CART");
                if (carts == null)
                {
                    carts = new List<CartItem>();
                }
                return carts;
            }
        }
        public IActionResult AddToCart(int id, int qty = 1)
        {
            var gioHang = Cart;

            //kiểm tra id (MaHH) truyền qua đã nằm trong giỏ hàng hay chưa
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null) //đã có
            {
                item.SoLuong += qty;
            }
            else
            {
                var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)//id không có trong Database
                {
                    return RedirectToAction("Index", "HangHoas");
                }
                item = new CartItem
                {
                    MaHh = id,
                    SoLuong = qty,
                    TenHh = hangHoa.TenHh,
                    Hinh = hangHoa.Hinh,
                    DonGia = hangHoa.DonGia.Value
                };
                //thêm vào giỏ hàng
                gioHang.Add(item);
            }
            //cập nhật session
            HttpContext.Session.Set("CART", gioHang);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set("CART", gioHang);
            }
            return RedirectToAction("Index");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
