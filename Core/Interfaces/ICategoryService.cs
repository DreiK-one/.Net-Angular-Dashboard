using Core.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(CategoryDto categoryDto, IFormFile file);
        Task<int> DeleteCategory(int id);
        Task<Category> UpdateCategory(CategoryDto categoryDto, IFormFile file);
    }
}
