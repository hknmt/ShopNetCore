using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BanHang.ViewModel.ProductViewModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");

            var result = await _productService.GetProductById(id.Value);

            if (result == null)
                return RedirectToAction("Index", "Home");

            var model = new IndexViewModel {
                ProductId = result.ProductId,
                ProductName = result.ProductName,
                ProductDescription = result.ProductDescription,
                ProductImage = result.ProductImage,
                ProductPrice = result.ProductPrice,
                ProductQuantity = result.ProductQuantity
            };

            var productRelated = await _productService.GetProductRelated(id.Value);

            ViewBag.ProductRelated = productRelated
                .Select(x => new ProductRelated {
                    ProductId = x.ProductId,
                    ProductImage = x.ProductImage,
                    ProductName = x.ProductName,
                    ProductPrice = x.ProductPrice
                });

            await _productService.AddView(id.Value);

            return View(model);
        }
    }
}