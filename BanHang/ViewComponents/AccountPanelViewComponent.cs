using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewComponents
{
    public class AccountPanelViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly ICustomerService _customerService;

        public AccountPanelViewComponent(IHttpContextAccessor httpContextAccessor, ICustomerService customerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _customerService = customerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var UserId = _session.GetInt32("CustomerId").Value;
            var customer = await _customerService.GetCustomerById(UserId);
            ViewData["CustomerFullname"] = customer.CustomerFullname;
            return View();
        }
    }
}
