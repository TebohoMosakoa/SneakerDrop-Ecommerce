using SneakerDrop.Database;
using SneakerDrop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SneakerDrop.Services
{
    public class OrdersService
    {
        #region Singleton
        public static OrdersService Instance
        {
            get
            {
                if (instance == null) instance = new OrdersService();

                return instance;
            }
        }
        private static OrdersService instance { get; set; }

        private OrdersService()
        {
             
        }

        #endregion

        public List<Order> SearchOrders(string userID, string status, int pageNo, int pageSize)
        {
            using (var context = new SDContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public Order GetOrderByID(int iD)
        {
            using (var context = new SDContext())
            {
                return context.Orders.Where(x=>x.ID == iD).Include(x=>x.OrderItems).Include("OrderItems.Product").FirstOrDefault();
            }
        }

        public int SearchOrdersCount(string userID, string status)
        {
            using (var context = new SDContext())
            {
                var orders = context.Orders.ToList();

                if (!string.IsNullOrEmpty(userID))
                {
                    orders = orders.Where(x => x.UserID.ToLower().Contains(userID.ToLower())).ToList();
                }

                if (!string.IsNullOrEmpty(status))
                {
                    orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
                }

                return orders.Count;
            }
        }

        public bool UpdateOrderStatus(int iD, string status)
        {
            using (var context = new SDContext())
            {
                var order = context.Orders.Find(iD);
                order.Status = status;
                context.Entry(order).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }
    }
}
