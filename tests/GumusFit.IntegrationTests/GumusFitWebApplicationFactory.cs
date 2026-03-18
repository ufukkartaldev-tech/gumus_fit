using GumusFit.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GumusFit.IntegrationTests;

public class GumusFitWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<GumusFitDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<GumusFitDbContext>>();

            services.AddDbContext<GumusFitDbContext>(options =>
                options.UseInMemoryDatabase("GumusFitIntegrationTestDb"));
        });
    }
}
