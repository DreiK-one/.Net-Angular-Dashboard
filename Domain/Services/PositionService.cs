using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class PositionService : IPositionService
    {
        private readonly ApiContext _context;

        public PositionService(ApiContext context)
        {
            _context = context;
        }

        public async Task<List<Position>> GetPositionsByCategoryId(int id)
        {
            try
            {
                var positions = await _context.Positions
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == id)
                    .ToListAsync();

                return positions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int> CreatePosition(Position position)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeletePosition(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdatePosition(Position position)
        {
            throw new NotImplementedException();
        }
    }
}
