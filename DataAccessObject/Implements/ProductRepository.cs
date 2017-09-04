using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using DataAccessObject.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DataAccessObject.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly BanHangDbContext _context;
        private readonly IConfigRepository _config;

        public ProductRepository(BanHangDbContext context, IConfigRepository config)
        {
            _context = context;
            _config = config;
        }

        public void AddView(int ProductId)
        {
            var Product = _context.Product.Where(x => x.ProductId == ProductId).FirstOrDefault();

            Product.ProductViewed += 1;

            _context.SaveChanges();
        }

        public long CountProduct(int? CategoryId, string ProductName, bool? Status)
        {
            var conn = _context.Database.GetDbConnection();
            var count = 0;
            var where = false;
            try
            {
                conn.Open();
                using(var command = conn.CreateCommand())
                {
                    string query = "SELECT COUNT(0) FROM [Product]";
                    
                    if(CategoryId != null)
                    {
                        query += " WHERE CategoryId = " + CategoryId;
                        where = true;
                    }

                    if (!string.IsNullOrWhiteSpace(ProductName))
                    {
                        if (where)
                            query += " AND";
                        else
                        {
                            query += " WHERE";
                            where = true;
                        }

                        query += " ProductName LIKE '%" + ProductName;
                    }

                    

                    if (Status != null)
                    {
                        if (where)
                            query += " AND";
                        else
                            query += " WHERE";

                        query += " ProductStatus = " + (Status == true ? 1 : 0);
                    }
                    
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
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

        public void Create(Product productData)
        {
            _context.Product.Add(productData);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var result = _context.Product.FirstOrDefault(x => x.ProductId == id);

            if (result != null)
            {
                _context.Product.Remove(result);
                _context.SaveChanges();
            }
        }

        public void Edit(Product productData, int id)
        {
            var result = _context.Product.FirstOrDefault(x => x.ProductId == id);

            if(result != null)
            {
                result.CategoryId = productData.CategoryId;
                result.ProductName = productData.ProductName;
                result.ProductImage = productData.ProductImage;
                result.ProductPrice = productData.ProductPrice;
                result.ProductQuantity = productData.ProductQuantity;
                result.ProductStatus = productData.ProductStatus;
            }

            _context.SaveChanges();
        }

        public IEnumerable<Product> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status)
        {
            int pageSize = int.Parse(_config.GetValueByName("PageSize"));
            int currentPage = Page ?? 1;
            int skipRow = (currentPage - 1) * pageSize;

            if (Status == null)
            {
                if (CategoryId == null)
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return _context.Product
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                    else
                    {
                        return _context.Product
                            .Where(x => x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                    else
                    {
                        return _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                }
            }
            else
            {
                if (CategoryId == null)
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return _context.Product
                            .Where(x => x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                    else
                    {
                        return _context.Product
                            .Where(x => x.ProductStatus == Status)
                            .Where(x => x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                    else
                    {
                        return _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductName.Contains(ProductName) && x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToList();
                    }
                }
            }
        }

        public Product GetProductById(int id)
        {
            return _context.Product.FirstOrDefault(x => x.ProductId == id);
        }

        public IEnumerable<Product> GetProductRelated(int ProductId)
        {
            var Category = _context.Product.Where(x => x.ProductId == ProductId).FirstOrDefault();

            var Related = _context.Product
                .Where(x => x.CategoryId == Category.CategoryId && x.ProductStatus == true && x.ProductId != ProductId)
                .OrderBy(x => x.ProductViewed)
                .Take(10)
                .ToList();

            return Related;
        }
    }
}
