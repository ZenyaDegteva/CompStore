using ComputerShop.Filters;
using ComputerShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
    [Authorize(Roles = "Client")]
    [InitializeSimpleMembership]
    public class CartController : Controller
    {

        private ComputerShopDBEntities db = new ComputerShopDBEntities();
        order order = new order();

        public ActionResult Index()
        {
            Cart cart = getCart();
            List<product> products = new List<product>();
            cart.Content.Keys.ToList().ForEach(key => products.Add(db.product.Find(key)));
            ViewBag.products = products;
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int id = 0)
        {
            //ищем товар по его id
            product product = db.product.Find(id);
            //если нашли
            if (product != null)
            {
                //находим корзину и добавляем наш товар, то есть его id
                getCart().AddProduct(id);
                //добавляем в список заказов, чтобы была потом возможность оформить
                order.product.Add(product);
            }
            //в противном случае - возвращаем страницу HttpNotFound()
            else
                return HttpNotFound();
            //return PartialView(product);
            //получаем нашу корзину сессионную с содержимым
            var content = getCart().Content;
            //число творвар в корзине
            int prCount = 0;
            //если количество товаров в корзине больше 1
            if (content.Any())
            {
                //узнаем количество товаров в корзине
                prCount = content.Count();
            }
            //возвращаем имя товара, которого добавили в корзину и количество товаров в корзине
            return Json(new { ProductName = product.product_name, ProductCount = prCount });
        }

        public ActionResult Remove(int id = 0)
        {
            //находим товар
            product product = db.product.Find(id);
            if (product != null)
            {
                //если все ок, находим корзину и удаляем товар (если количество одного и того товара больше 1, то удалится количество)
                // противном случае, удалится весь товар (product) из корзины
                getCart().RemoveProduct(id);
                //потом удаляем его из таблицы Order
                order.product.Remove(product);
            }
            else
                return HttpNotFound();
            //после всех операций делаем возврат (redirect) на Index, то есть отобразятся товары в нашей корзине
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Clear()
        {
            //удалятся все товары из нашей корзины
            return View();
        }

        [HttpPost, ActionName("Clear")]
        public ActionResult ClearConfirmed()
        {
            ((Cart)Session["Cart"]).Clear();
            order.product.Clear();
            return RedirectToAction("Index");
        }

        private Cart getCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}
