using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using BanHang.Areas.Admin.Models.SalesViewModel;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace BanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Sales")]
    [Authorize]
    public class SalesController : Controller
    {
        private readonly IOrderService _orderService;

        public SalesController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public async Task<IActionResult> Index()
        {
            ViewData["TotalOrder"] = await _orderService.Count();

            return View();
        }

        [HttpGet]
        [Route("[action]/{id?}")]
        public async Task<IActionResult> OrderDetail(int? id)
        {
            if (id == null)
                return BadRequest();

            var result = await _orderService.GetOrderDetail(id.Value);

            if (result == null)
                return BadRequest();

            var model = result.Select(x => new OrderDetailViewModel
            {
                OrderId = x.OrderId,
                Product = x.Product,
                ProductId = x.ProductId,
                ProductPrice = x.ProductPrice,
                ProductQuantity = x.ProductQuantity
            });

            ViewBag.Order = await _orderService.GetOrderById(id.Value);

            if (ViewBag.Order == null)
                return BadRequest();

            return View(model);
        }

        [HttpPost]
        [Route("[action]/{Status?}/{OrderId?}")]
        public async Task<IActionResult> OrderConfirm(bool? Status, int? OrderId)
        {
            if (Status == null || OrderId == null)
                return RedirectToAction("Index", "Sales");

            var result = await _orderService.GetOrderById(OrderId.Value);

            if(result == null)
                return RedirectToAction("Index", "Sales");

            if(result.OrderStatus == null)
            {
                await _orderService.SetOrderConfirm(OrderId.Value, Status.Value);
            }

            return RedirectToAction("Index", "Sales");
        }

        [HttpGet]
        [Route("[action]/{page?}")]
        public async Task<PartialViewResult> GetListOrders(int? page)
        {
            var result = await _orderService.GetOrders(page);

            var model = result.Select(x => new ListOrdersViewModel {
                CreateAt = x.CreateAt,
                Customer = x.Customer,
                CustomerId =x.CustomerId,
                OrderId = x.OrderId,
                OrderStatus = x.OrderStatus,
                ShipAddress = x.ShipAddress,
                ShipName = x.ShipName,
                ShipPhone =x.ShipPhone
            });

            Hashtable Total = new Hashtable();

            foreach (var item in model)
            {
                var orderDetail = await _orderService.GetOrderDetail(item.OrderId);
                decimal sum = 0;
                foreach (var detail in orderDetail)
                    sum += detail.ProductPrice * detail.ProductQuantity;
                Total.Add(item.OrderId, sum);
            }

            ViewBag.Total = Total;

            return PartialView("~/Areas/Admin/Views/Sales/_GetListOrdersPartial.cshtml", model);
        }
    }
}