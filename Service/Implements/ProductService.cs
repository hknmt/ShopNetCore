using BusinessObject;
using DataAccessObject.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

        public bool AddCart(int ProductId, int Quantity)
        {
            var Product = _productRepository.GetProductById(ProductId);

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

        public void AddView(int ProductId)
        {
            _productRepository.AddView(ProductId);
        }

        public long CountProduct(int? CategoryId, string ProductName, bool? Status)
        {
            return _productRepository.CountProduct(CategoryId, ProductName, Status);
        }

        public void Create(Product productData)
        {
            _productRepository.Create(productData);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        public void Edit(Product productData, int id)
        {
            _productRepository.Edit(productData, id);
        }

        public IEnumerable<Product> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status)
        {
            return _productRepository.GetListProduct(CategoryId, ProductName, Page, Status);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        public IEnumerable<Product> GetProductRelated(int ProductId)
        {
            return _productRepository.GetProductRelated(ProductId);
        }
    }
}
