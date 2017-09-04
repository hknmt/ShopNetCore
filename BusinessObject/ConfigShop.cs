using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessObject
{
    public class ConfigShop
    {
        [Key]
        public int ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
