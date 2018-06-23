using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository productRepository;

        public AdminController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View(productRepository.Products);
        }

        public ViewResult Edit(int productID)
        {
            var product = productRepository.Products.SingleOrDefault(p => p.ProductID == productID);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values                
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public RedirectToActionResult Delete(int productID)
        {
            var product = productRepository.Delete(productID);
            if (product != null)
            {
                TempData["message"] = $"{product.Name} has been deleted";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}