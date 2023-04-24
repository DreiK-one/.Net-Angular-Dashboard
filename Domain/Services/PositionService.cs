using Core.DTOs;
using Core.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


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

        public async Task<int> CreatePosition(PositionDto positionDto)
        {
            try
            {
                if (positionDto == null)
                {
                    throw new NullReferenceException();
                }

                var position = new Position
                {
                    Name = positionDto.Name,
                    Cost = positionDto.Cost,
                    CategoryId = positionDto.CategoryId,
                    CreatedByUser = positionDto.CreatedByUserId
                };

                await _context.Positions.AddAsync(position);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }     
        }

        public async Task<int> DeletePosition(int id)
        {
            try
            {
                var position = await _context.Positions
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (position == null)
                {
                    throw new NullReferenceException();
                }

                _context.Remove(position);
                return await _context.SaveChangesAsync();  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdatePosition(PositionDto positionDto)
        {
            if (positionDto == null)
            {
                throw new NullReferenceException();
            }

            var position = new Position
            {
                Name = positionDto.Name,
                Cost = positionDto.Cost
            };

            _context.Positions.Update(position);
            return await _context.SaveChangesAsync();
        }
    }
}
