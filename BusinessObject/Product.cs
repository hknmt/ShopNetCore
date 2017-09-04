using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BusinessObject
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string ProductImage { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public string ProductDescription { get; set; }

        public int ProductViewed { get; set; }

        public bool ProductStatus { get; set; }

        public virtual Category Category { get; set; }
    }
}
