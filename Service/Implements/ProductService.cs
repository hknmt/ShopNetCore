using BusinessObject;
using DataAccessObject.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Service.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfigRepository _configRepository;

        public ProductService(IProductRepository productRepository, IConfigRepository configRepository)
        {
            _productRepository = productRepository;
            _configRepository = configRepository;
        }

        public async Task<bool> AddCart(int ProductId, int Quantity)
        {
            var Product = await _productRepository.GetProductById(ProductId);

            if (Product == null)
                return false;

            if (Quantity > Product.ProductQuantity)
                return false;

            var Cart = new
            {
                ProductId = ProductId,
                Quantity = Quantity
            };

            return true;
        }

        public async Task AddView(int ProductId)
        {
            await _productRepository.AddView(ProductId);
        }

        public async Task<int> CountProduct(int? CategoryId, string ProductName, bool? Status)
        {
            return await _productRepository.CountProduct(CategoryId, ProductName, Status);
        }

        public async Task Create(Product productData)
        {
            await _productRepository.Create(productData);
        }

        public async Task Delete(int id)
        {
            await _productRepository.Delete(id);
        }

        public async Task Edit(Product productData, int id)
        {
           await _productRepository.Edit(productData, id);
        }

        public async Task<IEnumerable<Product>> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status)
        {
            return await _productRepository.GetListProduct(CategoryId, ProductName, Page, Status);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task<IEnumerable<Product>> GetProductRelated(int ProductId)
        {
            return await _productRepository.GetProductRelated(ProductId);
        }
    }
}
