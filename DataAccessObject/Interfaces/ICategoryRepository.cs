using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObject.Interfaces
{
    public interface ICategoryRepository
    {
        int Create(Category category);
        Category GetCategoryById(int id);
        IEnumerable<Category> GetListCategory();
        bool CheckCategoryExist(string CategoryName);
        void Delete(int id);
        void Edit(Category categoryData, int id);
    }
}
