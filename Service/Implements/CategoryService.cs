using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using DataAccessObject.Interfaces;
using DataAccessObject.Redis.Interfaces;
using Service.Interfaces;

namespace Service.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRedisRepository _redisRepository;

        public CategoryService(ICategoryRepository categoryRepository, IRedisRepository redisRepository)
        {
            _categoryRepository = categoryRepository;
            _redisRepository = redisRepository;
        }

        public bool CheckCategoryExist(string CategoryName)
        {
            return _categoryRepository.CheckCategoryExist(CategoryName);
        }

        public void Create(Category category)
        {
            var result = _categoryRepository.Create(category);

            _redisRepository.Set("Category_" + result, category, 10);
        }

        public void Delete(int id)
        {
            _categoryRepository.Delete(id);
        }

        public void Edit(Category categoryData, int id)
        {
            _categoryRepository.Edit(categoryData, id);
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public IEnumerable<Category> GetListCategory()
        {
            return _categoryRepository.GetListCategory();
        }
    }
}
