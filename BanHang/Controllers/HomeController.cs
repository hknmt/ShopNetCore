using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BanHang.ViewModel.CategoryViewModel;

namespace BanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;

        public HomeController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult GetCategoryList()
        {
            var result = _categoryService.GetListCategory().Select(x => new CategoryListViewModel {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            return PartialView("~/Views/Shared/_MenuCategory.cshtml", result);
        }
    }
}
