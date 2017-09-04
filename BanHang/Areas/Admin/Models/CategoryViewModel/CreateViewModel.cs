using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Areas.Admin.Models.CategoryViewModel
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống")]
        [MinLength(3, ErrorMessage = "Ít nhất 3 ký tự")]
        [Remote("CheckCategoryExist", "Category", "Admin", ErrorMessage = "Tên category đang được sử dụng")]
        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }
    }
}
