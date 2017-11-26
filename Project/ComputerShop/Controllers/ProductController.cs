﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComputerShop.Models;

namespace ComputerShop.Controllers
{
    public class ProductController : Controller
    {
        private ComputerShopDBEntities db = new ComputerShopDBEntities();
        //
        // GET: /Product/

        public ActionResult Index(String searchString)
        {
            //выборка всех продуктов
            var products = from product in db.product
                           select product;

            int price = 0;
            //если @try = true, значит searchString - число (цена), если нет - название товара либо категории
            bool @try = int.TryParse(searchString, out price);

            //если searchString не пуст и @try - не число, значит фильтруем по категории либо по названию товара
            if (!String.IsNullOrEmpty(searchString) && !@try)
            {
                products = products.Where(s => s.product_name.Contains(searchString) || s.category.Contains(searchString));
            }
            //в противном случае, если searchString не пуст и @try - число, значит, фильтруем по цене
            else if (!String.IsNullOrEmpty(searchString) && @try)
            {
                products = products.Where(s => s.price <= price);
            }
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            ViewBag.productOrder_id = new SelectList(db.order, "order_id", "payment_method");

            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(product product)
        {
            if (ModelState.IsValid)
            {
                db.product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productOrder_id = new SelectList(db.order, "order_id", "payment_method", product.productOrder_id);
            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.productOrder_id = new SelectList(db.order, "order_id", "payment_method", product.productOrder_id);
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productOrder_id = new SelectList(db.order, "order_id", "payment_method", product.productOrder_id);
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.product.Find(id);
            db.product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}