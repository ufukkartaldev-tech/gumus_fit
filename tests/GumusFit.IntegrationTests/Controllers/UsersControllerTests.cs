using System.Net;
using System.Net.Http.Json;
using GumusFit.Application.DTOs;
using GumusFit.Data.Contexts;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace GumusFit.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<GumusFitWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly GumusFitWebApplicationFactory _factory;

    public UsersControllerTests(GumusFitWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ReturnsSeededUsers()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GumusFitDbContext>();
        db.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Username = "integration_user",
            Email = "integration@test.com",
            Role = UserRole.User
        });
        await db.SaveChangesAsync();

        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(users);
        Assert.Contains(users!, u => u.Username == "integration_user");
    }
}
