using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFullname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerPassword { get; set; }
    }
}
