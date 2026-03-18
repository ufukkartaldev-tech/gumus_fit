using Microsoft.EntityFrameworkCore;
using GumusFit.Data.Repositories;
using GumusFit.Domain.Interfaces;
using GumusFit.Application.Mappings;
using GumusFit.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<GumusFit.Data.Contexts.GumusFitDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICalorieRepository, CalorieRepository>();

// Add Application Layer
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICalorieService, CalorieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
