using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.Areas.Admin.Models.SalesViewModel
{
    public class IndexViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ShipName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipAddress { get; set; }
        public bool? OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
