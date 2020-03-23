using SneakerDrop.Services;
using SneakerDrop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SneakerDrop.Web.Controllers
{
    public class HomeController : Controller
    {
        CategoriesService service = new CategoriesService();
        ProductsService productsService = new ProductsService();
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.FeaturedCategories = service.GetFeaturedCategories();
            model.FeaturedProducts = productsService.GetProducts(1, 8);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}