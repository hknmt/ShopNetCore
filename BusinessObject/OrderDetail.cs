using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessObject
{
    public class OrderDetail
    {
        [Key, ForeignKey("Order")]
        public int OrderId { get; set; }
        [Key, ForeignKey("Product")]
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
