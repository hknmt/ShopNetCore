using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Order data);
        Task CreateOrderDetail(OrderDetail data);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<OrderDetail>> GetOrderDetail(int OrderId);
        Task SetOrderConfirm(int OrderId, bool OrderStatus);
    }
}
