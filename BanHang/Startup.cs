using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using DataAccessObject;
using DataAccessObject.Implements;
using DataAccessObject.Interfaces;
using DataAccessObject.Redis.Interfaces;
using DataAccessObject.Redis.Implements;
using Service.Implements;
using Service.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BusinessObject;
using StackExchange.Redis;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanHang
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // Add connectionstring
            services.AddSingleton(Configuration);

            services.AddDbContext<BanHangDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString
                ("DefaultConnection")));

            services.AddIdentity<MyIdentityUser, MyIdentityRole>()
                .AddEntityFrameworkStores<BanHangDbContext>()
                .AddDefaultTokenProviders();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.CookieHttpOnly = true;
            });

            // Add Service Redis
            var ConnectionMutiplexer = ConnectionMultiplexer.Connect("127.0.0.1");
            var Database = ConnectionMutiplexer.GetDatabase(0);
            services.AddTransient<IDatabase>(_ => Database);


            // Add framework services.
            services.AddMvc();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // Cookie settings
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Admin/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Admin/Account/LogOut";
                options.Cookies.ApplicationCookie.AccessDeniedPath = "/Admin/Account/AccessDenied";
                // User settings
                options.User.RequireUniqueEmail = false;
            });

            // DI
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IConfigRepository, ConfigRepository>();
            services.AddTransient<IRedisRepository, RedisRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IFileMagager, FileManager>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IOrderService, OrderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
