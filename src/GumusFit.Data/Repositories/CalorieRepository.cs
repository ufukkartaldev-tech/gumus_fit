using GumusFit.Data.Contexts;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GumusFit.Data.Repositories;

public class CalorieRepository : ICalorieRepository
{
    private readonly GumusFitDbContext _context;

    public CalorieRepository(GumusFitDbContext context)
    {
        _context = context;
    }

    public async Task<CalorieEntry?> GetByIdAsync(Guid id)
    {
        return await _context.CalorieEntries.FindAsync(id);
    }

    public async Task<IReadOnlyList<CalorieEntry>> GetAllAsync()
    {
        return await _context.CalorieEntries.ToListAsync();
    }

    public async Task<IReadOnlyList<CalorieEntry>> GetByUserIdAsync(Guid userId)
    {
        return await _context.CalorieEntries
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(CalorieEntry entry)
    {
        await _context.CalorieEntries.AddAsync(entry);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CalorieEntry entry)
    {
        _context.CalorieEntries.Update(entry);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CalorieEntry entry)
    {
        _context.CalorieEntries.Remove(entry);
        await _context.SaveChangesAsync();
    }
}
