using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize { get; set; } = 4;

        public ProductController(IProductRepository repo) { repository = repo; }

        public ViewResult List(string category, int productPage = 1)
        {
            var totalProducts = repository.Products;
            var products = totalProducts
                            .Where(p=> category == null || p.Category == category)
                            .OrderBy(p => p.ProductID)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize);

            var count = category == null ? totalProducts.Count() : totalProducts.Where(c => c.Category == category).Count();

            var pagingInfo = new PagingInfo { TotalItems = count, CurrentPage = productPage, ItemsPerPage = PageSize };

            return View(new ProductsListViewModel { Products = products, PagingInfo = pagingInfo, CurrentCategory = category });
        }
    }
}
