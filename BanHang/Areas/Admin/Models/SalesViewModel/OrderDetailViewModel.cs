using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Areas.Admin.Models.SalesViewModel
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
