using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BanHang.ViewModel.AccountViewModel;
using Service.Interfaces;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BanHang.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public AccountController(ICustomerService customerService, IHttpContextAccessor httpContextAccessor)
        {
            _customerService = customerService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            if (_session.GetInt32("CustomerId") != null)
                return RedirectToAction("Index", "Home");

            ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel data)
        {
            if (ModelState.IsValid) {
                var model = await _customerService.CheckLogin(data.CustomerEmail, data.CustomerPassword);

                if(model == null)
                {
                    ModelState.AddModelError("", "Sai Email hoặc Mật khẩu");
                    return PartialView("~/Views/Account/_LoginPartial.cshtml", data);
                }

                _session.SetInt32("CustomerId", model.CustomerId);
                _session.SetString("CustomerEmail", model.CustomerEmail);

                return Json(new {
                    success = "ok"
                });
            }
            else
            {
                return PartialView("~/Views/Account/_LoginPartial.cshtml", data);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel data)
        {
            var customer = new Customer
            {
                CustomerEmail = data.CustomerEmail,
                CustomerFullname = data.CustomerFullname,
                CustomerPassword = data.CustomerPassword,
                CustomerAddress = data.CustomerAddress,
                CustomerPhone = data.CustomerPhone
            };

            var result = await _customerService.Create(customer);

            _session.SetInt32("CustomerId", result.CustomerId);
            _session.SetString("CustomerEmail", result.CustomerEmail);

            return Ok();
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateInformation()
        {
            var customerId = _session.GetInt32("CustomerId").Value;

            var result = await _customerService.GetCustomerById(customerId);

            var model = new UpdateInformationViewModel
            {
                CustomerFullname = result.CustomerFullname,
                CustomerPhone = result.CustomerPhone,
                CustomerAddress = result.CustomerAddress
            };

            return PartialView("~/Views/Account/_UpdateInformationPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInformation(UpdateInformationViewModel data)
        {
            var model = new Customer
            {
                CustomerId = _session.GetInt32("CustomerId").Value,
                CustomerAddress = data.CustomerAddress,
                CustomerPhone = data.CustomerPhone,
                CustomerFullname = data.CustomerFullname
            };

            var result = await _customerService.UpdateInformation(model);

            if (result)
                return Json(new
                {
                    success = true
                });

            return Json(new
            {
                success = false,
                message = "Cập nhật thất bại"
            });
        }

        public IActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}