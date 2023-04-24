using Core.DTOs;
using Data.Entities;


namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<Category> CreateCategory(CategoryDto categoryDto, string rootPath);
        Task<int> DeleteCategory(int id);
        Task<Category> UpdateCategory(CategoryDto categoryDto, string rootPath);
    }
}
