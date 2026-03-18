using GumusFit.Data.Contexts;
using GumusFit.Data.Repositories;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GumusFit.DataTests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly GumusFitDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<GumusFitDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new GumusFitDbContext(options);
        _repository = new UserRepository(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task AddAsync_SavesUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "alice", Email = "alice@test.com", Role = UserRole.User };

        await _repository.AddAsync(user);

        var saved = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.Equal("alice", saved!.Username);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "bob", Email = "bob@test.com", Role = UserRole.Admin };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal("bob", result!.Username);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        _context.Users.AddRange(
            new User { Id = Guid.NewGuid(), Username = "u1", Email = "u1@test.com", Role = UserRole.User },
            new User { Id = Guid.NewGuid(), Username = "u2", Email = "u2@test.com", Role = UserRole.User }
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ChangesArePersisted()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "old", Email = "old@test.com", Role = UserRole.User };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Username = "new";
        await _repository.UpdateAsync(user);

        var updated = await _context.Users.FindAsync(user.Id);
        Assert.Equal("new", updated!.Username);
    }

    [Fact]
    public async Task DeleteAsync_RemovesUser()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "del", Email = "del@test.com", Role = UserRole.User };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(user);

        var deleted = await _context.Users.FindAsync(user.Id);
        Assert.Null(deleted);
    }
}
