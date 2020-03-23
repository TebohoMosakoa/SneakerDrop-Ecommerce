using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerDrop.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime OrderedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
