using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;
using DataAccessObject.Interfaces;

namespace Service.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<Order> CreateOrder(Order data)
        {
            return await _orderRepository.Create(data);
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<IEnumerable<Order>> GetOrders(int? page)
        {
            return await _orderRepository.GetOrders(page);
        }

        public async Task CreateOrderDetail(OrderDetail data)
        {
            await _orderDetailRepository.Create(data);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetail(int OrderId)
        {
            return await _orderDetailRepository.GetOrderDetailByOrderId(OrderId);
        }

        public async Task SetOrderConfirm(int OrderId, bool OrderStatus)
        {
            if (OrderStatus)
            {
                var order = _orderDetailRepository.GetOrderDetailByOrderId(OrderId);
            }

            await _orderRepository.SetOrderConfirm(OrderId, OrderStatus);
        }

        public async Task<int> Count()
        {
            return await _orderRepository.Count();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int CustomerId, int? Page)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(CustomerId, Page);

            foreach(var order in orders)
            {
                var orderDetail = await _orderDetailRepository.GetOrderDetailByOrderId(order.OrderId);
                order.OrderDetail = orderDetail;
            }

            return orders;
        }
    }
}
