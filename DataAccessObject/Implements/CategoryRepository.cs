using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataAccessObject.Interfaces;
using System.Threading.Tasks;

namespace DataAccessObject.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BanHangDbContext _context;

        public CategoryRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Category category)
        {
            var result = await _context.Category.AddAsync(category);
            
            await _context.SaveChangesAsync();

            return result.Entity.CategoryId;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetListCategory()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<bool> CheckCategoryExist(string CategoryName)
        {
            var result = await _context.Category.Where(x => x.CategoryName == CategoryName).FirstOrDefaultAsync();

            if (result == null)
                return true;

            return false;
        }

        public async Task Delete(int id)
        {
            var Category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == id);

            if (Category != null)
            {
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Edit(Category categoryData, int id)
        {
            var Category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == id);

            if(Category != null)
            {
                Category.CategoryName = categoryData.CategoryName;
            }

            await _context.SaveChangesAsync();
        }
    }
}
