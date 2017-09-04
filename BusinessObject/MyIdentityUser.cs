using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BusinessObject
{
    public class MyIdentityUser : IdentityUser
    {
        public DateTime UserBirthday { get; set; }
        public string UserAvatar { get; set; }
        public bool UserGender { get; set; }
    }
}
