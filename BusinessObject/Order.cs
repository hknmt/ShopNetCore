using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string ShipName { get; set; }
        public string ShipPhone { get; set; }
        public string ShipAddress { get; set; }
        public bool? OrderStatus { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
