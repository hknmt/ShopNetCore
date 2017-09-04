using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Interfaces;
using BanHang.ViewModel.CategoryViewModel;

namespace BanHang.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = _categoryService.GetListCategory().Select(x => new CategoryListViewModel {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName
            });

            return View(result);
        }
    }
}
