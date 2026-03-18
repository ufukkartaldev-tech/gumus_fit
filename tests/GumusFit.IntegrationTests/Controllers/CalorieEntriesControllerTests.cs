using System.Net;
using System.Net.Http.Json;
using GumusFit.Application.DTOs;
using GumusFit.Data.Contexts;
using GumusFit.Domain.Entities;
using GumusFit.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace GumusFit.IntegrationTests.Controllers;

public class CalorieEntriesControllerTests : IClassFixture<GumusFitWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly GumusFitWebApplicationFactory _factory;

    public CalorieEntriesControllerTests(GumusFitWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<User> SeedUserAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GumusFitDbContext>();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "calorie_user_" + Guid.NewGuid(),
            Email = $"{Guid.NewGuid()}@test.com",
            Role = UserRole.User
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/calorieentries");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_NonExisting_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/calorieentries/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreated()
    {
        var user = await SeedUserAsync();
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 600, Description = "Test meal" };

        var response = await _client.PostAsJsonAsync($"/api/calorieentries/user/{user.Id}", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<CalorieEntryDto>();
        Assert.NotNull(created);
        Assert.Equal(600, created!.Calories);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsEntry()
    {
        var user = await SeedUserAsync();
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 450 };
        var createResponse = await _client.PostAsJsonAsync($"/api/calorieentries/user/{user.Id}", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<CalorieEntryDto>();

        var response = await _client.GetAsync($"/api/calorieentries/{created!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var entry = await response.Content.ReadFromJsonAsync<CalorieEntryDto>();
        Assert.Equal(450, entry!.Calories);
    }

    [Fact]
    public async Task Update_ExistingId_ReturnsNoContent()
    {
        var user = await SeedUserAsync();
        var createDto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 100 };
        var createResponse = await _client.PostAsJsonAsync($"/api/calorieentries/user/{user.Id}", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<CalorieEntryDto>();

        var updateDto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 999 };
        var response = await _client.PutAsJsonAsync($"/api/calorieentries/{created!.Id}", updateDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 100 };

        var response = await _client.PutAsJsonAsync($"/api/calorieentries/{Guid.NewGuid()}", dto);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var user = await SeedUserAsync();
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 200 };
        var createResponse = await _client.PostAsJsonAsync($"/api/calorieentries/user/{user.Id}", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<CalorieEntryDto>();

        var response = await _client.DeleteAsync($"/api/calorieentries/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"/api/calorieentries/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByUserId_ReturnsUserEntries()
    {
        var user = await SeedUserAsync();
        var dto = new CreateCalorieEntryDto { Date = DateTime.Today, Calories = 350 };
        await _client.PostAsJsonAsync($"/api/calorieentries/user/{user.Id}", dto);

        var response = await _client.GetAsync($"/api/calorieentries/user/{user.Id}");
        var entries = await response.Content.ReadFromJsonAsync<List<CalorieEntryDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(entries!);
    }
}
