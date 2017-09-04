using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task Create(OrderDetail data);
        Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderId(int Id);
    }
}
