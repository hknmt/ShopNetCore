using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DataAccessObject.Interfaces;

namespace DataAccessObject.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BanHangDbContext _context;

        public CategoryRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public int Create(Category category)
        {
            var result = _context.Category.Add(category);
            
            _context.SaveChanges();

            return result.Entity.CategoryId;
        }

        public Category GetCategoryById(int id)
        {
            return _context.Category.FirstOrDefault(x => x.CategoryId == id);
        }

        public IEnumerable<Category> GetListCategory()
        {
            return _context.Category.ToList();
        }

        public bool CheckCategoryExist(string CategoryName)
        {
            var result = _context.Category.Where(x => x.CategoryName == CategoryName).FirstOrDefault();

            if (result == null)
                return true;

            return false;
        }

        public void Delete(int id)
        {
            var Category = _context.Category.FirstOrDefault(x => x.CategoryId == id);

            if (Category != null)
            {
                _context.Category.Remove(Category);
                _context.SaveChanges();
            }
        }

        public void Edit(Category categoryData, int id)
        {
            var Category = _context.Category.FirstOrDefault(x => x.CategoryId == id);

            if(Category != null)
            {
                Category.CategoryName = categoryData.CategoryName;
            }

            _context.SaveChanges();
        }
    }
}
