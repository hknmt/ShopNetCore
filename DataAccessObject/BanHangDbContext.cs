using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace DataAccessObject
{
    public class BanHangDbContext : IdentityDbContext<MyIdentityUser, MyIdentityRole, string>
    {
        public BanHangDbContext(DbContextOptions<BanHangDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(x => x.ProductViewed)
                .HasDefaultValue(0);

            modelBuilder.Entity<Product>()
                .Property(x => x.ProductDescription)
                .HasColumnType("ntext");

            modelBuilder.Entity<Order>()
                .Property(x => x.CreateAt)
                .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<OrderDetail>()
                .HasKey(x => new { x.OrderId, x.ProductId });
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ConfigShop> ConfigShop { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
