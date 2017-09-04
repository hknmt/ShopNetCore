using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewModel.CartViewModel
{
    public class OtherInformationViewModel
    {
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
    }
}
