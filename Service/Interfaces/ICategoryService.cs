using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICategoryService
    {
        Task Create(Category category);
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetListCategory();
        Task<bool> CheckCategoryExist(string CategoryName);
        Task Delete(int id);
        Task Edit(Category categoryData, int id);
    }
}
