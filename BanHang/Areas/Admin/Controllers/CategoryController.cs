using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BanHang.Areas.Admin.Models.CategoryViewModel;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;

namespace BanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetListCategory()
        {
            var result = _categoryService.GetListCategory();

            var model = result.Select(x => new IndexViewModel
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            return PartialView("~/Areas/Admin/Views/Category/PartialView/_ListCategory.cshtml", model);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("~/Areas/Admin/Views/Category/PartialView/_CreatePartial.cshtml");
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(CreateViewModel categoryData)
        {
            if (!_categoryService.CheckCategoryExist(categoryData.CategoryName))
            {
                ModelState.AddModelError("", "Tên category đang được sử dụng");
                return PartialView("~/Areas/Admin/Views/Category/PartialView/_CreatePartial.cshtml", categoryData);
            }

            var category = new Category {
                CategoryName = categoryData.CategoryName
            };

            _categoryService.Create(category);

            return Json(new {
                success = true
            });
        }

        [Route("Detail/{id?}/{type?}")]
        [HttpGet]
        public string Detail(int? id, int? type)
        {
            return id.ToString() + type.ToString();
        }

        [Route("[action]/{CategoryName?}/{CategoryNameOld?}")]
        [HttpGet]
        public JsonResult CheckCategoryExist(string CategoryName, string CategoryNameOld)
        {
            if(string.Compare(CategoryName, CategoryNameOld, true) == 0)
                return Json(true);

            return Json(_categoryService.CheckCategoryExist(CategoryName));
        }

        [Route("[action]/{id?}")]
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return Json(new {
                    success = false
                });

            _categoryService.Delete(id.Value);

            return Json(new
            {
                success = true
            });
        }

        [Route("[action]/{id?}")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
                return RedirectToAction("Index", "Category");

            var result = _categoryService.GetCategoryById(id.Value);

            if(result == null)
                return RedirectToAction("Index", "Category");

            TempData["CategoryId"] = id.Value;
            TempData["CategoryNameOld"] = result.CategoryName;

            var model = new EditViewModel
            {
                CategoryName = result.CategoryName,
                CategoryNameOld = result.CategoryName
            };

            return PartialView("~/Areas/Admin/Views/Category/PartialView/_EditPartial.cshtml", model);
        }

        [Route("[action]/{id?}")]
        [HttpPost]
        public IActionResult Edit(EditViewModel categoryData)
        {
            var CategoryId = (int)TempData["CategoryId"];
            var CategoryNameOld = (string)TempData["CategoryNameOld"];

            if (!ModelState.IsValid)
                return View(categoryData);

            if(string.Compare(categoryData.CategoryName, CategoryNameOld) == 0)
                return RedirectToAction("Index", "Category");

            var model = new Category
            {
                CategoryName = categoryData.CategoryName
            };

            _categoryService.Edit(model, CategoryId);

            return RedirectToAction("Index", "Category");

        }
    }
}