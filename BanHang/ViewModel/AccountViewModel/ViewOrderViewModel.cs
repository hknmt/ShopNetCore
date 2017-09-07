using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.AccountViewModel
{
    public class ViewOrderViewModel
    {
        public int OrderId { get; set; }
        public bool? OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public string ShipName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipAddress { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
