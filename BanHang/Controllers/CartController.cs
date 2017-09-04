using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BanHang.ViewModel.CartViewModel;
using Newtonsoft.Json;
using Service.Interfaces;
using BusinessObject;

namespace BanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;        

        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public CartController(IHttpContextAccessor httpContextAccessor, 
            IProductService productService, 
            ICustomerService customerService,
            IOrderService orderService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            if (String.IsNullOrEmpty(_session.GetString("Cart")))
                return View();

            decimal amount = 0;

            var session = JsonConvert.DeserializeObject<IEnumerable<ProductSession>>(_session.GetString("Cart"));

            var Products = new List<IndexViewModel>();

            foreach(var item in session)
            {
                var result = _productService.GetProductById(item.ProductId);

                amount += item.Quantity * result.ProductPrice;

                Products.Add(new IndexViewModel {
                    ProductId = item.ProductId,
                    ProductQuantity = item.Quantity,
                    ProductImage = result.ProductImage,
                    ProductName = result.ProductName,
                    ProductPrice = result.ProductPrice
                });
            }

            ViewData["Amount"] = amount;
            return View(Products);
        }

        public PartialViewResult OtherInformation()
        {
            return PartialView("~/Views/Account/_OtherInformationPartial.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OtherInformationViewModel data)
        {
            if (_session.GetString("Cart") == null)
                return Json(new {
                    success = false,
                    message = "Giỏ hàng trống"
                });

            if(_session.GetInt32("CustomerId") == null)
                return Json(new
                {
                    success = false,
                    message = "Chưa đăng nhập"
                });

            var customerId = _session.GetInt32("CustomerId").Value;

            var customer = await _customerService.GetCustomerById(customerId);

            var order = await _orderService.CreateOrder(new Order{
                CustomerId = customerId,
                ShipName = data.CustomerFullname ?? customer.CustomerFullname,
                ShipPhone = data.CustomerPhone ?? customer.CustomerPhone,
                ShipAddress = data.CustomerAddress ?? customer.CustomerAddress
            });

            var ListProduct = JsonConvert.DeserializeObject<IEnumerable<ProductSession>>(_session.GetString("Cart"));

            foreach(var item in ListProduct)
            {
                var product = _productService.GetProductById(item.ProductId);
                await _orderService.CreateOrderDetail(new OrderDetail {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    ProductPrice = product.ProductPrice,
                    ProductQuantity = item.Quantity
                });
            }

            _session.Remove("Cart");

            return Json(new
            {
                success = true,
                message = "Mua hàng thành công"
            });
        }

        public async Task<IActionResult> Shipping()
        {
            if (_session.GetString("CustomerId") == null)
                return RedirectToAction("Login", "Account", new { returnUrl = "/Cart/Shipping" });

            if(_session.GetString("Cart") == null)
                return RedirectToAction("Index", "Home");

            var CustomerId = _session.GetInt32("CustomerId").Value;

            var result = await _customerService.GetCustomerById(CustomerId);

            var model = new ShippingViewModel {
                CustomerFullname = result.CustomerFullname,
                CustomerAddress = result.CustomerAddress,
                CustomerPhone = result.CustomerPhone
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddToCart(int ProductId, int? Quantity)
        {
            var ProductObj = _productService.GetProductById(ProductId);

            if (ProductObj == null)
                return Json(new
                {
                    success = false,
                    message = "Đặt hàng thất bại"
                });

            if(ProductObj.ProductQuantity < (Quantity ?? 1))
                if (ProductObj == null)
                    return Json(new
                    {
                        success = false,
                        message = "Hết hàng"
                    });

            var Session = _session.GetString("Cart");

            if (string.IsNullOrEmpty(Session))
            {
                var Product = new ProductSession{
                    ProductId = ProductId,
                    Quantity = Quantity ?? 1
                };

                var ListObj = new List<ProductSession>();

                ListObj.Add(Product);

                _session.SetString("Cart", JsonConvert.SerializeObject(ListObj));

                return Json(new {
                    success = true,
                    message = "Đặt hàng thành công",
                    data = Product
                });
            }

            var List = JsonConvert.DeserializeObject<List<ProductSession>>(Session);

            var Index = List.FindIndex(x => x.ProductId == ProductId);

            if (Index == -1)
            {
                List.Add(new ProductSession
                {
                    ProductId = ProductId,
                    Quantity = Quantity ?? 1
                });

                _session.SetString("Cart", JsonConvert.SerializeObject(List));
            }
            else
            {
                List[Index].Quantity = Quantity.Value;
                _session.SetString("Cart", JsonConvert.SerializeObject(List));
            }

            return Json(new {
                success = true,
                message = "Đặt hàng thành công!",
                data = List
            });
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var session = _session.GetString("Cart");

            var result = JsonConvert.DeserializeObject<List<ProductSession>>(session);

            var Index = result.FindIndex(x => x.ProductId == id);

            if (Index == -1)
                return BadRequest();

            result.RemoveAt(Index);

            if (result.Count < 1)
                _session.Remove("Cart");
            else
                _session.SetString("Cart", JsonConvert.SerializeObject(result));

            return Ok();
        }
    }
}