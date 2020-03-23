using SneakerDrop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SneakerDrop.Web.ViewModels
{
    public class ProductWidgetViewModel
    {
        public List<Product> Products { get; set; }

        public bool IsLatestProducts { get; set; }

        public List<Category> Categories { get; set; }
    }
}