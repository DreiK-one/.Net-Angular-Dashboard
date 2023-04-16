using Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<int> CreateCategory(Category category);
        Task<int> DeleteCategory(int id);
        Task<int> UpdateCategory(Category category);

        Task<string> AddFile(IFormFile file);
    }
}
