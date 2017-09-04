using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> Create(Order data);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetOrders();
        Task SetOrderConfirm(int OrderId, bool OrderStatus);
    }
}
