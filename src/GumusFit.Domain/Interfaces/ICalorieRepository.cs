using GumusFit.Domain.Entities;

namespace GumusFit.Domain.Interfaces;

public interface ICalorieRepository
{
    Task<CalorieEntry?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<CalorieEntry>> GetAllAsync();
    Task<IReadOnlyList<CalorieEntry>> GetByUserIdAsync(Guid userId);
    Task AddAsync(CalorieEntry entry);
    Task UpdateAsync(CalorieEntry entry);
    Task DeleteAsync(CalorieEntry entry);
}
