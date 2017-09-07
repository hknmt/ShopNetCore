using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.AccountViewModel
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Họ và tên")]
        public string CustomerFullname { get; set; }

        [DisplayName("Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DisplayName("Địa chỉ")]
        [DataType(DataType.MultilineText)]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Nhập số điện thoại")]
        [DisplayName("Số điện thoại")]
        public string CustomerPhone { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string CustomerPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Nhập giống mật khẩu mới")]
        [DisplayName("Nhập lại")]
        public string ReNewPassword { get; set; }
    }
}
