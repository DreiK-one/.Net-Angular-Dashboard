using Core.DTOs;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApiContext _context;

        public CategoryService(ApiContext context)
        {
            _context = context; 
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

        public async Task<Category> CreateCategory(CategoryDto categoryDto, IFormFile file)
        {
            try
            {
                //if (file != null)
                //{
                //    Implement fileService adding file
                //}

                var newCategory = new Category
                {
                    Name = categoryDto.Name,
                    UserId = categoryDto.UserId,
                    //ImageSource = file ?? fileService.AddImage() : ""
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

        public Task<Category> UpdateCategory(CategoryDto categoryDto, IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
