using AutoMapper;
using GumusFit.Application.DTOs;
using GumusFit.Application.Mappings;
using GumusFit.Application.Services;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Enums;
using GumusFit.Domain.Interfaces;
using Moq;

namespace GumusFit.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _service = new UserService(_repoMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Username = "alice", Email = "alice@example.com", Role = UserRole.User },
            new() { Id = Guid.NewGuid(), Username = "bob",   Email = "bob@example.com",   Role = UserRole.Admin }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _service.GetAllUsersAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Username == "alice");
        Assert.Contains(result, u => u.Username == "bob");
    }

    [Fact]
    public async Task GetAllUsersAsync_EmptyList_ReturnsEmpty()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        var result = await _service.GetAllUsersAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_MapsRoleToString()
    {
        var users = new List<User>
        {
            new() { Id = Guid.NewGuid(), Username = "admin", Email = "admin@example.com", Role = UserRole.Admin }
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var result = await _service.GetAllUsersAsync();

        Assert.Equal("Admin", result[0].Role);
    }
}
