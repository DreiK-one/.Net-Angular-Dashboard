using Data.Entities;


namespace Core.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetPositionsByCategoryId(int id);
        Task<int> CreatePosition(Position position);
        Task<int> DeletePosition(int id);
        Task<int> UpdatePosition(Position position);
    }
}
