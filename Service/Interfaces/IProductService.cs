﻿using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;

namespace Service.Interfaces
{
    public interface IProductService
    {
        void Create(Product productData);
        IEnumerable<Product> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status);
        Product GetProductById(int id);
        void Edit(Product productData, int id);
        void Delete(int id);
        long CountProduct(int? CategoryId, string ProductName, bool? Status);
        IEnumerable<Product> GetProductRelated(int ProductId);
        void AddView(int ProductId);
        bool AddCart(int ProductId, int Quantity);
    }
}