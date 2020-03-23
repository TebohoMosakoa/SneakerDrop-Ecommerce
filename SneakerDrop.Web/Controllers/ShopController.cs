using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SneakerDrop.Entities;
using SneakerDrop.Services;
using SneakerDrop.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SneakerDrop.Web.Controllers
{
    public class ShopController : Controller
    {
        CheckoutViewModel model = new CheckoutViewModel();
        ProductsService service = new ProductsService();


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public  ActionResult Index(string searchTerm,int? categoryID,int? minimumPrice,int? maximumPrice, int? sortBy, int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();

            ShopViewModel model = new ShopViewModel();

            model.SearchTerm = searchTerm;
            model.FeaturedCategoies = CategoriesService.Instance.GetFeaturedCategories();
            model.MaximumPrice = service.GetMaximumPrice();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            model.Products = ProductsService.Instance.SearchProducts(searchTerm,minimumPrice,maximumPrice,categoryID, sortBy, pageNo.Value, pageSize);
            model.SortBy = sortBy;
            model.CategoryID = categoryID;
            int totalCount =  ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);
            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return View(model);
        }

        public ActionResult FilterProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();

            FilterProductsViewModel model = new FilterProductsViewModel();

            model.SearchTerm = searchTerm;
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;

            int totalCount = ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);
            model.Products = ProductsService.Instance.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);

            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return PartialView(model);
        }


        [Authorize]
        public ActionResult Checkout()
        {
            var CartProductsCookie = Request.Cookies["CartProducts"];
              
            if(CartProductsCookie != null && !string.IsNullOrEmpty(CartProductsCookie.Value) )
            {
                //var productIDs = CartProductsCookie.Value;

                //var ids = productIDs.Split('-');
                //List<int> prodIDS = ids.Select(x => int.Parse(x)).ToList();
                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
                model.CartProducts = service.GetProducts(model.CartProductIDs);
                model.User = UserManager.FindById(User.Identity.GetUserId());
            }

            return View(model);
        }

        public JsonResult PlaceOrder(string productIDS)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (!string.IsNullOrEmpty(productIDS))
            {
                var productQuantities = productIDS.Split('-').Select(x => int.Parse(x)).ToList();

                var boughtProducts = ProductsService.Instance.GetProducts(productQuantities.Distinct().ToList());

                Order newOrder = new Order();
                newOrder.UserID = User.Identity.GetUserId();
                newOrder.OrderedAt = DateTime.Now;
                newOrder.Status = "Pending";
                newOrder.TotalAmount = boughtProducts.Sum(x => x.Price * productQuantities.Where(productID => productID == x.Id).Count());

                newOrder.OrderItems = new List<OrderItem>();
                newOrder.OrderItems.AddRange(boughtProducts.Select(x => new OrderItem() { ProductID = x.Id, Quantity = productQuantities.Where(productID => productID == x.Id).Count() }));

                var rowsEffected = ShopService.Instance.SaveOrder(newOrder);

                result.Data = new { Success = true, Rows = rowsEffected };
            }
            else
            {
                result.Data = new { Success = false };
            }

            return result;
        }
    }
}