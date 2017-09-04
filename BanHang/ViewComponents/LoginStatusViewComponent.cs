using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BanHang.ViewComponents
{
    public class LoginStatusViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public LoginStatusViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_session.GetInt32("CustomerId") == null)
                return View();

            ViewData["CustomerEmail"] = _session.GetString("CustomerEmail");

            return View("LoggedIn");
        }
    }
}
