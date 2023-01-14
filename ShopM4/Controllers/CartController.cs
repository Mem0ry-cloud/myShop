using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Utility;

namespace ShopM4.Controllers
{
    public class CartController : Controller
    {
        // GET: CartController
        ApplicationDbContext db;
        public CartController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }
            List<int> productsIdInCart = cartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList = db.Product.Where(x => productsIdInCart.Contains(x.Id)).ToList();
            return View(productList);
        }
        public IActionResult Delete(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
