using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComputerShop.Models;
using WebMatrix.WebData;
using ComputerShop.Filters;
using ComputerShop.DAO;
namespace ComputerShop.Controllers
{
    [InitializeSimpleMembership]
    public class DeliveryController : Controller
    {
        private ComputerShopDBEntities db = new ComputerShopDBEntities();
        private DeliveryDAO deliveryDAO = new DeliveryDAO();


        [Authorize(Roles = "Client")]
        [HttpGet]
        public ActionResult Index()
        {
            var delivery = deliveryDAO.getDelivery(WebSecurity.CurrentUserId);
            ViewData.Model = delivery;
            return View();
        }

        [Authorize(Roles = "Client")]
        public ActionResult Details(int id = 0)
        {
            delivery delivery = db.delivery.Find(id);
            return View(delivery);
        }

        //
        // GET: /Delivery/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(delivery delivery)
        {
            deliveryDAO.CreateDelivery(delivery);
            return RedirectToAction("Index", "Home");
        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}