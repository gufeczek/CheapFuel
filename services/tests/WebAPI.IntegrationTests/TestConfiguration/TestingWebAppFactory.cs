using System.Linq;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.IntegrationTests.TestConfiguration;

public class TestingWebApiFactory<TEntity> : WebApplicationFactory<Program> where TEntity : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            services.AddScoped<IUserPrincipalService, UserPrincipalServiceTest>();
        });
    }
}

public class UserPrincipalServiceTest : IUserPrincipalService
{
    public int GetUserPrincipalId()
    {
        return -1;
    }
}