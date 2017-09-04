using DataAccessObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Implements
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BanHangDbContext _context;

        public OrderDetailRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public async Task Create(OrderDetail data)
        {
            await _context.OrderDetail.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderId(int Id)
        {
            return await _context.OrderDetail
                .Join(_context.Product, x => x.ProductId, y => y.ProductId, (x,y) => new OrderDetail
                {
                    ProductId = x.ProductId,
                    Product = y,
                    OrderId = x.OrderId,
                    ProductPrice = x.ProductPrice,
                    ProductQuantity = x.ProductQuantity
                })
                .Where(x => x.OrderId == Id).ToListAsync();
        }
    }
}
