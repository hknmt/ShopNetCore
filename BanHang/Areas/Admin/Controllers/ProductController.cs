using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BusinessObject;
using BanHang.Areas.Admin.Models.ProductViewModel;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IConfigService _configService;
        private readonly IFileMagager _fileManager;

        public ProductController(IProductService productService, 
            ICategoryService categoryService, 
            IConfigService configService,
            IFileMagager fileManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _configService = configService;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index()
        {
            var categoryModel = await _categoryService.GetListCategory();

            var categoryViewModel = categoryModel.Select(x => new BanHang.Areas.Admin.Models.CategoryViewModel.IndexViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            return View(categoryViewModel);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var listCategory = await _categoryService.GetListCategory();
            ViewBag.ListCategory = listCategory.Select(x => new BanHang.Areas.Admin.Models.CategoryViewModel.IndexViewModel {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel productData)
        {
            var listCategory = await _categoryService.GetListCategory();
            ViewBag.ListCategory = listCategory.Select(x => new BanHang.Areas.Admin.Models.CategoryViewModel.IndexViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            if (!ModelState.IsValid)
                return View(productData);

            var productModel = new Product
            {
                CategoryId = productData.CategoryId,
                ProductName = productData.ProductName,
                ProductImage = await _fileManager.Upload(productData.ProductImage, null, null, null),
                ProductPrice = productData.ProductPrice,
                ProductQuantity = productData.ProductQuantity,
                ProductStatus = productData.ProductStatus,
                ProductDescription = productData.ProductDescription
            };

            _productService.Create(productModel);

            return RedirectToAction("Index", "Product");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> GetListProduct(int? CategoryId, string ProductName, int? Page)
        {
            var result = await _productService.GetListProduct(CategoryId, ProductName, Page, null);
            var count = _productService.CountProduct(CategoryId, ProductName, null);

            ViewData["totalItem"] = count;
            ViewData["currentPage"] = Page ?? 1;
            ViewData["pageSize"] = _configService.GetValueByName("PageSize");

            var model = result.Select(x => new IndexViewModel {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductPrice = x.ProductPrice,
                ProductQuantity = x.ProductQuantity,
                ProductViewed = x.ProductViewed,
                ProductStatus = x.ProductStatus
            });

            return PartialView("~/Areas/Admin/Views/Product/PartialView/ListProduct.cshtml", model);
        }

        [Route("[action]")]
        [HttpPost]
        public JsonResult Delete(int? id)
        {
            if (id != null)
            {
                _productService.Delete(id.Value);
                return Json(new {
                    success = true
                });
            }

            return Json(new
            {
                success = false
            });
        }

        [Route("[action]/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Product");

            var listCategory = await _categoryService.GetListCategory();

            ViewBag.ListCategory = listCategory.Select(x => new BanHang.Areas.Admin.Models.CategoryViewModel.IndexViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            var result = await _productService.GetProductById(id.Value);

            var viewModel = new EditViewModel
            {
                ProductId = result.ProductId,
                CategoryId = result.CategoryId,
                ProductName = result.ProductName,
                ProductImage = result.ProductImage,
                ProductPrice = result.ProductPrice,
                ProductQuantity = result.ProductQuantity,
                ProductStatus = result.ProductStatus,
                ProductDescription = result.ProductDescription
            };

            return View(viewModel);
        }

        [Route("[action]/{id?}")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel viewModel, IFormFile file)
        {
            var image = viewModel.ProductImage;
            if (file != null)
                image = await _fileManager.Upload(file, null, null, null);

            var productData = new Product {
                ProductId = viewModel.ProductId,
                CategoryId = viewModel.CategoryId,
                ProductName = viewModel.ProductName,
                ProductImage = image,
                ProductPrice = viewModel.ProductPrice,
                ProductQuantity = viewModel.ProductQuantity,
                ProductStatus = viewModel.ProductStatus,
                ProductDescription = viewModel.ProductDescription
            };

            _productService.Edit(productData, viewModel.ProductId);
            return RedirectToAction("Index", "Product");
        }
    }
}