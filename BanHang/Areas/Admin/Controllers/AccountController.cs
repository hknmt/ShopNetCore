using BanHang.Areas.Admin.Models.AccountViewModel;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly RoleManager<MyIdentityRole> _roleManager;

        public AccountController(UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            RoleManager<MyIdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(CreateViewModel userData)
        {
            if (!ModelState.IsValid)
                return View(userData);

            var user = new MyIdentityUser {
                UserName = userData.UserName,
                UserBirthday = userData.Birthday,
                UserAvatar = "http://via.placeholder.com/45x45",
                UserGender = userData.Gender
            };

            var result = _userManager.CreateAsync(user, userData.Password).Result;

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Employee").Result)
                {
                    var role = new MyIdentityRole {
                        Name = "Employee",
                        Description = "Dành cho nhân viên"
                    };

                    var roleResult = _roleManager.CreateAsync(role).Result;

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Lỗi khi tạo quyền");

                        return View(userData);
                    }
                }

                _userManager.AddToRoleAsync(user, "Employee").Wait();

                return RedirectToAction("Login", "Account");
            }

            foreach (var item in result.Errors)
                ModelState.AddModelError("", item.Description);

            return View(userData);
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(obj.UserName, obj.Password, obj.Remember, true).Result;

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", "Sai mật khẩu hoặc tên đăng nhập");

                return View(obj);
            }

            return View(obj);
        }

        [Route("[action]")]
        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();

            return RedirectToAction("Login", "Account");
        }
    }
}