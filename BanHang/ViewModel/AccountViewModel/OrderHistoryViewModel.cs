using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.AccountViewModel
{
    public class OrderHistoryViewModel
    {
        public int OrderId { get; set; }
        public bool? OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public decimal OrderTotal { get; set; }
        public int NumberItem { get; set; }
        public string ItemFirst { get; set; }
    }
}
