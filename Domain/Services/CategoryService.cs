using Core.Interfaces;
using Data;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApiContext _context;

        public CategoryService(ApiContext context)
        {
            _context = context; 
        }

        public Task<List<Category>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
