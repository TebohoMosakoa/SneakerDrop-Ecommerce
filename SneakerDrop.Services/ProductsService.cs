using SneakerDrop.Database;
using SneakerDrop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerDrop.Services
{
    public class ProductsService
    {
        #region Singleton
        public static ProductsService Instance
        {
            get
            {
                if (instance == null) instance = new ProductsService();

                return instance;
            }
        }
        private static ProductsService instance { get; set; }

        public ProductsService()
        {
        }

        #endregion

        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy, int pageNo, int pageSize)
        {
            using (var context = new SDContext())
            {
                var products = context.Products.ToList();

                if (categoryId.HasValue)
                {
                    products = products.Where(x => x.Category.Id == categoryId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                    }
                }

                return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public int SearchProductsCount(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryId, int? sortBy)
        {
            using (var context = new SDContext())
            {
                var products = context.Products.ToList();

                if (categoryId.HasValue)
                {
                    products = products.Where(x => x.Category.Id == categoryId.Value).ToList();
                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
                }

                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.Id).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        default:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;
                    }
                }

                return products.Count;
            }
        }


        public int GetMaximumPrice()
        {
            using (var context = new SDContext())
            {
                return (int)(context.Products.Max(x => x.Price));
            }
        }

        public Product GetProduct(int Id)
        {
            using (var context = new SDContext())
            {
                return context.Products.Where(x => x.Id == Id).Include(x => x.Category).FirstOrDefault();
            }
        }

        public List<Product> GetProducts(List<int> Ids)
        {
            using (var context = new SDContext())
            {
                return context.Products.Where(product => Ids.Contains(product.Id)).ToList();
            }
        }

        public List<Product> GetProducts(int pageNo)
        {
            int pageSize = 5;// int.Parse(ConfigurationsService.Instance.GetConfig("ListingPageSize").Value);

            using (var context = new SDContext())
            {
                return context.Products.OrderBy(x => x.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();

            }
        }

        public List<Product> GetProducts(int pageNo, int pageSize)
        {
            using (var context = new SDContext())
            {
                return context.Products.OrderByDescending(x => x.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetProducts(string search, int pageNo, int pageSize)
        {
            using (var context = new SDContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                         product.Name.ToLower().Contains(search.ToLower()))
                         .OrderBy(x => x.Id)
                         .Skip((pageNo - 1) * pageSize)
                         .Take(pageSize)
                         .Include(x => x.Category)
                         .ToList();
                }
                else
                {
                    return context.Products
                        .OrderBy(x => x.Id)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Category)
                        .ToList();
                }
            }
        }

        public int GetProductsCount(string search)
        {
            using (var context = new SDContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products.Where(product => product.Name != null &&
                         product.Name.ToLower().Contains(search.ToLower()))
                         .Count();
                }
                else
                {
                    return context.Products.Count();
                }
            }
        }

        public List<Product> GetProductsByCategory(int categoryId, int pageSize)
        {
            using (var context = new SDContext())
            {
                return context.Products.Where(x => x.Category.Id == categoryId).OrderByDescending(x => x.Id).Take(pageSize).Include(x => x.Category).ToList();
            }
        }

        public List<Product> GetLatestProducts(int numberOfProducts)
        {
            using (var context = new SDContext())
            {
                return context.Products.OrderByDescending(x => x.Id).Take(numberOfProducts).Include(x => x.Category).ToList();
            }
        }

        public void SaveProduct(Product product)
        {
            using (var context = new SDContext())
            {
                context.Entry(product.Category).State = System.Data.Entity.EntityState.Unchanged;

                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var context = new SDContext())
            {
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteProduct(int Id)
        {
            using (var context = new SDContext())
            {
                var product = context.Products.Find(Id);

                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
