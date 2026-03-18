using AutoMapper;
using GumusFit.Application.DTOs;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Interfaces;

namespace GumusFit.Application.Services;

public class CalorieService : ICalorieService
{
    private readonly ICalorieRepository _calorieRepository;
    private readonly IMapper _mapper;

    public CalorieService(ICalorieRepository calorieRepository, IMapper mapper)
    {
        _calorieRepository = calorieRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CalorieEntryDto>> GetAllAsync()
    {
        var entries = await _calorieRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<CalorieEntryDto>>(entries);
    }

    public async Task<IReadOnlyList<CalorieEntryDto>> GetByUserIdAsync(Guid userId)
    {
        var entries = await _calorieRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IReadOnlyList<CalorieEntryDto>>(entries);
    }

    public async Task<CalorieEntryDto?> GetByIdAsync(Guid id)
    {
        var entry = await _calorieRepository.GetByIdAsync(id);
        return entry is null ? null : _mapper.Map<CalorieEntryDto>(entry);
    }

    public async Task<CalorieEntryDto> CreateAsync(Guid userId, CreateCalorieEntryDto dto)
    {
        var entry = new CalorieEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Date = dto.Date,
            Calories = dto.Calories,
            Description = dto.Description
        };

        await _calorieRepository.AddAsync(entry);
        return _mapper.Map<CalorieEntryDto>(entry);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateCalorieEntryDto dto)
    {
        var entry = await _calorieRepository.GetByIdAsync(id);
        if (entry is null) return false;

        entry.Date = dto.Date;
        entry.Calories = dto.Calories;
        entry.Description = dto.Description;

        await _calorieRepository.UpdateAsync(entry);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entry = await _calorieRepository.GetByIdAsync(id);
        if (entry is null) return false;

        await _calorieRepository.DeleteAsync(entry);
        return true;
    }
}
