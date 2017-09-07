using DataAccessObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DataAccessObject.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BanHangDbContext _context;

        public OrderRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public async Task<int> Count()
        {
            var conn = _context.Database.GetDbConnection();
            var count = 0;

            try
            {
                await conn.OpenAsync();
                using(var command = conn.CreateCommand())
                {
                    string query = "SELECT COUNT(0) FROM [Order]";
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        count = reader.GetInt32(0);
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public async Task<Order> Create(Order data)
        {
            var result = await _context.Order.AddAsync(data);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Order
                .Join(_context.Customer, o => o.CustomerId, c => c.CustomerId, (o,c) => new Order{
                    CustomerId = o.CustomerId,
                    Customer = c,
                    CreateAt = o.CreateAt,
                    OrderId = o.OrderId,
                    OrderStatus = o.OrderStatus,
                    ShipAddress = o.ShipAddress,
                    ShipName = o.ShipName,
                    ShipPhone = o.ShipPhone
                })
                .Where(x => x.OrderId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders(int? page)
        {
            return await _context.Order.Join(_context.Customer, x => x.CustomerId, y => y.CustomerId, (x, y) => new Order {
                OrderId = x.OrderId,
                CustomerId = x.CustomerId,
                CreateAt = x.CreateAt,
                ShipAddress = x.ShipAddress,
                ShipName = x.ShipName,
                ShipPhone = x.ShipPhone,
                OrderStatus = x.OrderStatus,
                Customer = y
            })
            .OrderByDescending(x => x.CreateAt)
            .Skip(10*((page ?? 1)-1))
            .Take(10)
            .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int CustomerId, int? Page)
        {
            return await _context.Order
                .Join(_context.Customer, x => x.CustomerId, y => y.CustomerId, (x, y) => new Order{
                    CustomerId = x.CustomerId,
                    CreateAt = x.CreateAt,
                    Customer = y,
                    OrderId = x.OrderId,
                    OrderStatus = x.OrderStatus,
                    ShipAddress = x.ShipAddress,
                    ShipName = x.ShipName,
                    ShipPhone = x.ShipPhone
                })
                .Where(x => x.CustomerId == CustomerId)
                .OrderByDescending(x => x.CreateAt)
                .Skip(10 * ((Page ?? 1) - 1))
                .Take(10)                
                .ToListAsync();
        }

        public async Task SetOrderConfirm(int OrderId, bool OrderStatus)
        {
            var order = await _context.Order.Where(x => x.OrderId == OrderId).FirstOrDefaultAsync();
            order.OrderStatus = OrderStatus;
            await _context.SaveChangesAsync();
        }
    }
}
