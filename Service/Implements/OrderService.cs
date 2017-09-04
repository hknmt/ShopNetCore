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
        private readonly IProductRepository _productRepository;

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

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetOrders();
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
    }
}
