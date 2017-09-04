using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface ICategoryService
    {
        void Create(Category category);
        Category GetCategoryById(int id);
        IEnumerable<Category> GetListCategory();
        bool CheckCategoryExist(string CategoryName);
        void Delete(int id);
        void Edit(Category categoryData, int id);
    }
}
