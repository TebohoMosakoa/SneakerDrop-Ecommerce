using SneakerDrop.Entities;
using SneakerDrop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SneakerDrop.Web.ViewModels
{
    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; }
        public string UserID { get; set; }
        public Pager Pager { get; set; }
        public string Status { get; set; }
    }

    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public ApplicationUser OrderedBy { get; set; }
        public List<string> AvailableStatuses { get; set; }
    }
}