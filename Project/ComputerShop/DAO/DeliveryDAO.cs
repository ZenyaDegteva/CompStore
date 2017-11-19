using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Data.EntityModel;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using ComputerShop.Models;

namespace ComputerShop.DAO
{
    public class DeliveryDAO
    {
        private ComputerShopDBEntities _entities = new ComputerShopDBEntities();

        public IEnumerable<delivery> getDelivery(int id)
        {
            int orderId =
                (
                from order in _entities.order
                where order.userorder_id == id
                select order.order_id
                )
                .FirstOrDefault();
            if (orderId > 0)
                return (
                    from delivery in _entities.delivery
                    where delivery.orderdelivery_id == orderId
                    select delivery);
            else
                throw new Exception("Ouups");
        }

        public int getDeliveryForCreate(int id)
        {
            return
                (
                from c in _entities.order
                where c.userorder_id == id
                select c.order_id
                )
                .FirstOrDefault();
        }
        public bool deleteDelivery(int id)
        {
            try
            {
                int idDel =
                (
                from c in _entities.delivery
                where c.orderdelivery_id == id
                select c.delivery_id
                )
                .FirstOrDefault();
           
                delivery delivery = _entities.delivery.Find(idDel);
                _entities.delivery.Remove(delivery);
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool confirmToDelivery(int id)
        {
            try
            {
                int idDel = 
                    (
                    from c in _entities.delivery
                    where c.orderdelivery_id == id
                    select c.delivery_id
                    )
                    .FirstOrDefault();            
                delivery delivery = _entities.delivery.Find(idDel);
                delivery.delivery_status = "отправлен";
                _entities.Entry(delivery).State = EntityState.Modified;
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}