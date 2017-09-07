using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BanHang.ViewModel.CategoryViewModel;

namespace BanHang.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IConfigService _configService;

        public CategoryController(ICategoryService categoryService, IProductService productService, IConfigService configService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _configService = configService;
        }

        public async Task<IActionResult> Index(int? CategoryId, int? Page)
        {
            if(CategoryId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Category = await _categoryService.GetCategoryById(CategoryId.Value);
            ViewData["TotalItem"] = await _productService.CountProduct(CategoryId, null, true);
            ViewData["CurrentPage"] = Page ?? 1;
            ViewData["PageSize"] = 10;
            ViewData["CategoryId"] = CategoryId;

            var result = await _productService.GetListProduct(CategoryId, null, Page, true);

            var model = result.Select(x => new IndexViewModel {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    ProductPrice = x.ProductPrice
            });

            ViewData["TotalItem"] = await _productService.CountProduct(CategoryId, null, true);

            return View(model);
        }
    }
}