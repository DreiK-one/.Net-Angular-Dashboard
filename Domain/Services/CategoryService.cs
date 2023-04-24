using Core.DTOs;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApiContext _context;
        private readonly IFileService _fileService;

        public CategoryService(ApiContext context, 
            IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Positions)
                    .ToListAsync();

                return categories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Positions)
                    .FirstOrDefaultAsync(c => c.Id == id);

                return category;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> CreateCategory(CategoryDto categoryDto, string rootPath)
        {
            try
            {
                string? testPath = null;

                if (categoryDto.file != null)
                {
                    testPath = await _fileService.AddFile(categoryDto.file, rootPath);
                }

                var newCategory = new Category
                {
                    Name = categoryDto.Name,
                    UserId = Convert.ToInt32(categoryDto.UserId),
                    ImageSource = testPath
                };

                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                var result = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryDto.Name);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    throw new NullReferenceException();
                }

                _context.Categories.Remove(category);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> UpdateCategory(CategoryDto categoryDto, string rootPath)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryDto.Id);

                if (categoryDto.file != null && categoryDto.ImageSource != category.ImageSource)
                {
                    category.ImageSource = await _fileService.AddFile(categoryDto.file, rootPath);
                }

                category.Name = categoryDto.Name;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return category;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
