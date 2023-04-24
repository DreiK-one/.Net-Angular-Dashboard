using Core.DTOs;
using Data.Entities;

using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(CategoryDto categoryDto);
        Task<int> DeleteCategory(int id);
        Task<Category> UpdateCategory(CategoryDto categoryDto);
    }
}
