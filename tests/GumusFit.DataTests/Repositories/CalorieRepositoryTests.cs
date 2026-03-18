using GumusFit.Data.Contexts;
using GumusFit.Data.Repositories;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GumusFit.DataTests.Repositories;

public class CalorieRepositoryTests : IDisposable
{
    private readonly GumusFitDbContext _context;
    private readonly CalorieRepository _repository;
    private readonly User _user;

    public CalorieRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<GumusFitDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new GumusFitDbContext(options);
        _repository = new CalorieRepository(_context);

        _user = new User { Id = Guid.NewGuid(), Username = "test", Email = "test@test.com", Role = UserRole.User };
        _context.Users.Add(_user);
        _context.SaveChanges();
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task AddAsync_SavesEntry()
    {
        var entry = new CalorieEntry { Id = Guid.NewGuid(), Calories = 500, Date = DateTime.Today, UserId = _user.Id };

        await _repository.AddAsync(entry);

        var saved = await _context.CalorieEntries.FindAsync(entry.Id);
        Assert.NotNull(saved);
        Assert.Equal(500, saved!.Calories);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsEntry()
    {
        var entry = new CalorieEntry { Id = Guid.NewGuid(), Calories = 300, Date = DateTime.Today, UserId = _user.Id };
        _context.CalorieEntries.Add(entry);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(entry.Id);

        Assert.NotNull(result);
        Assert.Equal(300, result!.Calories);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntries()
    {
        _context.CalorieEntries.AddRange(
            new CalorieEntry { Id = Guid.NewGuid(), Calories = 100, Date = DateTime.Today, UserId = _user.Id },
            new CalorieEntry { Id = Guid.NewGuid(), Calories = 200, Date = DateTime.Today, UserId = _user.Id }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsOnlyUserEntries()
    {
        var otherUser = new User { Id = Guid.NewGuid(), Username = "other", Email = "other@test.com", Role = UserRole.User };
        _context.Users.Add(otherUser);
        _context.CalorieEntries.AddRange(
            new CalorieEntry { Id = Guid.NewGuid(), Calories = 400, Date = DateTime.Today, UserId = _user.Id },
            new CalorieEntry { Id = Guid.NewGuid(), Calories = 500, Date = DateTime.Today, UserId = otherUser.Id }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetByUserIdAsync(_user.Id);

        Assert.Single(result);
        Assert.Equal(400, result[0].Calories);
    }

    [Fact]
    public async Task UpdateAsync_ChangesArePersisted()
    {
        var entry = new CalorieEntry { Id = Guid.NewGuid(), Calories = 200, Date = DateTime.Today, UserId = _user.Id };
        _context.CalorieEntries.Add(entry);
        await _context.SaveChangesAsync();

        entry.Calories = 999;
        await _repository.UpdateAsync(entry);

        var updated = await _context.CalorieEntries.FindAsync(entry.Id);
        Assert.Equal(999, updated!.Calories);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntry()
    {
        var entry = new CalorieEntry { Id = Guid.NewGuid(), Calories = 150, Date = DateTime.Today, UserId = _user.Id };
        _context.CalorieEntries.Add(entry);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(entry);

        var deleted = await _context.CalorieEntries.FindAsync(entry.Id);
        Assert.Null(deleted);
    }
}
