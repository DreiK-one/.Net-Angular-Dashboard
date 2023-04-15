using Core.DTOs;
using Data.Entities;


namespace Core.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetPositionsByCategoryId(int id);
        Task<int> CreatePosition(PositionDto positionDto);
        Task<int> DeletePosition(int id);
        Task<int> UpdatePosition(PositionDto positionDto);
    }
}
