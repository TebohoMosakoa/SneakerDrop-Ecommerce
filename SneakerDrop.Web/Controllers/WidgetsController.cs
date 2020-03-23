using SneakerDrop.Services;
using SneakerDrop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SneakerDrop.Web.Controllers
{
    public class WidgetsController : Controller
    {
        public ActionResult Products(bool isLatestProducts, int? CategoryID)
        {
            ProductWidgetViewModel model = new ProductWidgetViewModel();
            model.IsLatestProducts = isLatestProducts;
            if (isLatestProducts)
            {
                model.Products = ProductsService.Instance.GetLatestProducts(12);
            }
            else if(CategoryID.HasValue && CategoryID.Value>0)
            {
                model.Products = ProductsService.Instance.GetProductsByCategory(CategoryID.Value, 4);
            }
            else
            {
                model.Products = ProductsService.Instance.GetProducts(1, 8);
            }
            return PartialView(model);
        }
    }
}