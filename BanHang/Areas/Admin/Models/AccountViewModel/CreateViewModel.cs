using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BanHang.Areas.Admin.Models.AccountViewModel
{
    public class CreateViewModel
    {
        [Required]
        [MinLength(3, ErrorMessage = "Ít nhất 3 ký tự")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Nhập trùng với ô mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public bool Gender { get; set; }
    }
}
