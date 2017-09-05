using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;

namespace DataAccessObject.Interfaces
{
    public interface IProductRepository
    {
        Task Create(Product productData);
        Task<IEnumerable<Product>> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status);
        Task<Product> GetProductById(int id);
        Task Edit(Product productData, int id);
        Task Delete(int id);
        Task<int> CountProduct(int? CategoryId, string ProductName, bool? Status);
        Task<IEnumerable<Product>> GetProductRelated(int ProductId);
        Task AddView(int ProductId);
    }
}
