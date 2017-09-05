using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using DataAccessObject.Interfaces;
using DataAccessObject.Redis.Interfaces;
using Service.Interfaces;
using System.Threading.Tasks;

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

        public async Task<bool> CheckCategoryExist(string CategoryName)
        {
            return await _categoryRepository.CheckCategoryExist(CategoryName);
        }

        public async Task Create(Category category)
        {
            var result = await _categoryRepository.Create(category);

            _redisRepository.Set("Category_" + result, category, 10);
        }

        public async Task Delete(int id)
        {
            await _categoryRepository.Delete(id);
        }

        public async Task Edit(Category categoryData, int id)
        {
            await _categoryRepository.Edit(categoryData, id);
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryRepository.GetCategoryById(id);
        }

        public async Task<IEnumerable<Category>> GetListCategory()
        {
            return await _categoryRepository.GetListCategory();
        }
    }
}
