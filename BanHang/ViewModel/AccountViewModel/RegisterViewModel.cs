using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Bạn nhập Email")]
        [DisplayName("Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Họ và tên")]
        public string CustomerFullname { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Địa chỉ")]
        [DataType(DataType.MultilineText)]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Nhập số điện thoại")]
        [DisplayName("Số điện thoại")]
        public string CustomerPhone { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Mật khẩu")]
        public string CustomerPassword { get; set; }
    }
}
