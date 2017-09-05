using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using DataAccessObject.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Threading.Tasks;

namespace DataAccessObject.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly BanHangDbContext _context;

        public ProductRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public async Task AddView(int ProductId)
        {
            var Product = await _context.Product.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();

            Product.ProductViewed += 1;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CountProduct(int? CategoryId, string ProductName, bool? Status)
        {
            var conn = _context.Database.GetDbConnection();
            var count = 0;
            var where = false;
            try
            {
                await conn.OpenAsync();
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
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        count = reader.GetInt32(0);
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

        public async Task Create(Product productData)
        {
            await _context.Product.AddAsync(productData);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var result = await _context.Product.FirstOrDefaultAsync(x => x.ProductId == id);

            if (result != null)
            {
                _context.Product.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(Product productData, int id)
        {
            var result = await _context.Product.FirstOrDefaultAsync(x => x.ProductId == id);

            if(result != null)
            {
                result.CategoryId = productData.CategoryId;
                result.ProductName = productData.ProductName;
                result.ProductImage = productData.ProductImage;
                result.ProductPrice = productData.ProductPrice;
                result.ProductQuantity = productData.ProductQuantity;
                result.ProductStatus = productData.ProductStatus;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetListProduct(int? CategoryId, string ProductName, int? Page, bool? Status)
        {
            int pageSize = 10;
            int currentPage = Page ?? 1;
            int skipRow = (currentPage - 1) * pageSize;

            if (Status == null)
            {
                if (CategoryId == null)
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return await _context.Product
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        return await _context.Product
                            .Where(x => x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return await _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        return await _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                }
            }
            else
            {
                if (CategoryId == null)
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return await _context.Product
                            .Where(x => x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        return await _context.Product
                            .Where(x => x.ProductStatus == Status)
                            .Where(x => x.ProductName.Contains(ProductName))
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ProductName))
                    {
                        return await _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                    else
                    {
                        return await _context.Product
                            .Where(x => x.CategoryId == CategoryId.Value && x.ProductName.Contains(ProductName) && x.ProductStatus == Status)
                            .OrderBy(x => x.ProductName)
                            .Skip(skipRow)
                            .Take(pageSize).ToListAsync();
                    }
                }
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(x => x.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetProductRelated(int ProductId)
        {
            var Category = await _context.Product.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();

            var Related = await _context.Product
                .Where(x => x.CategoryId == Category.CategoryId && x.ProductStatus == true && x.ProductId != ProductId)
                .OrderBy(x => x.ProductViewed)
                .Take(10)
                .ToListAsync();

            return Related;
        }
    }
}
