using AutoMapper;
using GumusFit.Application.DTOs;
using GumusFit.Application.Mappings;
using GumusFit.Application.Services;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Interfaces;
using Moq;

namespace GumusFit.UnitTests.Services;

public class CalorieServiceTests
{
    private readonly Mock<ICalorieRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly CalorieService _service;

    public CalorieServiceTests()
    {
        _repoMock = new Mock<ICalorieRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _service = new CalorieService(_repoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntries()
    {
        var entries = new List<CalorieEntry>
        {
            new() { Id = Guid.NewGuid(), Calories = 500, Date = DateTime.Today, UserId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), Calories = 800, Date = DateTime.Today, UserId = Guid.NewGuid() }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entries);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var entry = new CalorieEntry { Id = id, Calories = 300, Date = DateTime.Today, UserId = Guid.NewGuid() };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entry);

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(300, result!.Calories);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CalorieEntry?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsUserEntries()
    {
        var userId = Guid.NewGuid();
        var entries = new List<CalorieEntry>
        {
            new() { Id = Guid.NewGuid(), Calories = 400, Date = DateTime.Today, UserId = userId },
            new() { Id = Guid.NewGuid(), Calories = 600, Date = DateTime.Today, UserId = userId }
        };
        _repoMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(entries);

        var result = await _service.GetByUserIdAsync(userId);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task CreateAsync_AddsEntryAndReturnsDto()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 700, Description = "Lunch" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CalorieEntry>())).Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(userId, dto);

        Assert.Equal(700, result.Calories);
        Assert.Equal("Lunch", result.Description);
        _repoMock.Verify(r => r.AddAsync(It.Is<CalorieEntry>(e => e.UserId == userId && e.Calories == 700)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesAndReturnsTrue()
    {
        var id = Guid.NewGuid();
        var existing = new CalorieEntry { Id = id, Calories = 200, Date = DateTime.Today, UserId = Guid.NewGuid() };
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 999, Description = "Updated" };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<CalorieEntry>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(id, dto);

        Assert.True(result);
        _repoMock.Verify(r => r.UpdateAsync(It.Is<CalorieEntry>(e => e.Calories == 999)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CalorieEntry?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new CreateCalorieEntryDto());

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DeletesAndReturnsTrue()
    {
        var id = Guid.NewGuid();
        var existing = new CalorieEntry { Id = id, Calories = 100, Date = DateTime.Today, UserId = Guid.NewGuid() };
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.DeleteAsync(It.IsAny<CalorieEntry>())).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(id);

        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CalorieEntry?)null);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }
}
