using BusinessObject;
using System;

namespace BanHang.Areas.Admin.Models.SalesViewModel
{
    public class ListOrdersViewModel
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
