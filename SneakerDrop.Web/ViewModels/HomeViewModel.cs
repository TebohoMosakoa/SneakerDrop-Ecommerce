using SneakerDrop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SneakerDrop.Web.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<Category> FeaturedCategories { get; set; }
    }
}