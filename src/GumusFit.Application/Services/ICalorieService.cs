using GumusFit.Application.DTOs;

namespace GumusFit.Application.Services;

public interface ICalorieService
{
    Task<IReadOnlyList<CalorieEntryDto>> GetAllAsync();
    Task<IReadOnlyList<CalorieEntryDto>> GetByUserIdAsync(Guid userId);
    Task<CalorieEntryDto?> GetByIdAsync(Guid id);
    Task<CalorieEntryDto> CreateAsync(Guid userId, CreateCalorieEntryDto dto);
    Task<bool> UpdateAsync(Guid id, CreateCalorieEntryDto dto);
    Task<bool> DeleteAsync(Guid id);
}
