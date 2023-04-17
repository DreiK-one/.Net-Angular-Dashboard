﻿using Core.Interfaces;
using Data;
using Data.Entities;
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

        public Task<int> CreateCategory(Category category)
        {
            throw new NotImplementedException();
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

        public Task<int> UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
