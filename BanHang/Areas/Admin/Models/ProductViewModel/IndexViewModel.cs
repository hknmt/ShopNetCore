using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Areas.Admin.Models.ProductViewModel
{
    public class IndexViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public int ProductViewed { get; set; }

        public bool ProductStatus { get; set; }
    }
}
