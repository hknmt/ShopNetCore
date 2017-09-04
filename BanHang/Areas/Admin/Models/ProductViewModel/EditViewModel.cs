using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Areas.Admin.Models.ProductViewModel
{
    public class EditViewModel
    {
        public int ProductId { get; set; }
        [DisplayName("Danh mục")]
        public int CategoryId { get; set; }

        [MinLength(3, ErrorMessage = "Ít nhất 3 ký tự")]
        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Không được để trống")]
        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Nhập sai định dạng")]
        [DisplayName("Giá sản phẩm")]
        [Required(ErrorMessage = "Không được để trống")]
        public decimal ProductPrice { get; set; }

        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Nhập sai định dạng")]
        [DisplayName("Số lượng sản phẩm")]
        [Required(ErrorMessage = "Không được để trống")]
        public int ProductQuantity { get; set; }

        [DisplayName("Mô tả sản phẩm")]
        public string ProductDescription { get; set; }

        [DisplayName("Tình trạng sản phẩm")]
        public bool ProductStatus { get; set; }
    }
}
