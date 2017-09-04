using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BusinessObject
{
    public class MyIdentityRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
