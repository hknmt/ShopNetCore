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
        private readonly IOrderService _orderService;

        public AccountController(ICustomerService customerService, IHttpContextAccessor httpContextAccessor, IOrderService orderService)
        {
            _customerService = customerService;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            if (_session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Account/OrderHistory" });

            return RedirectToAction("OrderHistory", "Account");
        }

        public async Task<IActionResult> ViewOrder(int? OrderId)
        {
            if (OrderId == null)
                return RedirectToAction("OrderHistory", "Account");

            if (_session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Account/OrderHistory" });

            var order = await _orderService.GetOrderById(OrderId.Value);

            if(order == null)
                return RedirectToAction("OrderHistory", "Account");

            var model = new ViewOrderViewModel
            {
                OrderId = order.OrderId,
                CreateAt = order.CreateAt,
                OrderStatus = order.OrderStatus,
                ShipAddress = order.ShipAddress,
                ShipName = order.ShipName,
                ShipPhone = order.ShipPhone,
                OrderDetail = await _orderService.GetOrderDetail(order.OrderId)
            };

            return View(model);
        }

        public async Task<IActionResult> OrderHistory(int? page)
        {
            if (_session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Account/OrderHistory" });

            var CustomerId = _session.GetInt32("CustomerId").Value;
            var orders = await _orderService.GetOrdersByCustomerId(CustomerId, page);
            var model = orders.Select(x => new OrderHistoryViewModel
            {
                CreateAt = x.CreateAt,
                OrderId = x.OrderId,
                OrderStatus = x.OrderStatus,
                ItemFirst = x.OrderDetail.First().Product.ProductName,
                NumberItem = x.OrderDetail.Count(),
                OrderTotal = x.OrderDetail.Sum(y => y.ProductQuantity * y.ProductPrice)
            });
            return View(model);
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

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            if(_session.GetInt32("CustomerId") == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Account/Edit" });

            var CustomerId = _session.GetInt32("CustomerId").Value;
            var customer = await _customerService.GetCustomerById(CustomerId);

            var model = new EditViewModel
            {
                CustomerAddress = customer.CustomerAddress,
                CustomerFullname = customer.CustomerFullname,
                CustomerPassword = customer.CustomerPassword,
                CustomerPhone = customer.CustomerPhone,
                CustomerEmail = customer.CustomerEmail
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel data)
        {
            if(!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    message = "Thay đổi thông tin thất bại"
                });

            var CustomerId = _session.GetInt32("CustomerId").Value;

            var customer = await _customerService.GetCustomerById(CustomerId);

            if (!string.IsNullOrWhiteSpace(data.NewPassword))
            {
                if (customer.CustomerPassword != data.CustomerPassword)
                    return Json(new
                    {
                        success = false,
                        message = "Sai mật khẩu"
                    });

                await _customerService.UpdatePassword(CustomerId, data.NewPassword);
            }

            customer.CustomerAddress = data.CustomerAddress;
            customer.CustomerFullname = data.CustomerFullname;
            customer.CustomerPhone = data.CustomerPhone;

            await _customerService.UpdateInformation(customer);

            return Json(new
            {
                success = true,
                message = "Thay đổi thông tin thành công"
            });
        }
    }
}