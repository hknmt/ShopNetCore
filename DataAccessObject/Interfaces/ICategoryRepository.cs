using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataAccessObject.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> Create(Category category);
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetListCategory();
        Task<bool> CheckCategoryExist(string CategoryName);
        Task Delete(int id);
        Task Edit(Category categoryData, int id);
    }
}
