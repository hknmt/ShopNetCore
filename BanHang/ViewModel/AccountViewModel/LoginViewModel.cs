using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Bạn nhập Email")]
        [DisplayName("Email")]
        public string CustomerEmail { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Mật khẩu")]
        public string CustomerPassword { get; set; }
    }
}
