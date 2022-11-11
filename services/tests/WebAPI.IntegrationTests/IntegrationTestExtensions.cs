using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebAPI.IntegrationTests.PredefinedData;

namespace WebAPI.IntegrationTests;

public static class IntegrationTestExtensions
{
    public static StringContent Serialize(this IntegrationTest integrationTest, object obj)
        => new(JsonConvert.SerializeObject(obj), Encoding.Default, MediaTypeNames.Application.Json);

    public static async Task<TEntity?> Deserialize<TEntity>(this IntegrationTest _, HttpContent content) where TEntity : class
        => JsonConvert.DeserializeObject<TEntity>(await content.ReadAsStringAsync());

    public static async Task AuthorizeUser(this IntegrationTest integrationTest)
        => await integrationTest.AuthorizeGenericUser(AccountsData.UserUsername, AccountsData.DefaultPassword);

    public static async Task AuthorizeOwner(this IntegrationTest integrationTest)
        => await integrationTest.AuthorizeGenericUser(AccountsData.OwnerUsername, AccountsData.DefaultPassword);

    public static async Task AuthorizeAdmin(this IntegrationTest integrationTest)
        => await integrationTest.AuthorizeGenericUser(AccountsData.AdminUsername, AccountsData.DefaultPassword);

    public static async Task AuthorizeGenericUser(this IntegrationTest integrationTest, string username, string password)
    {
        var command = new AuthenticateUserCommand(username, password);
        var response = await integrationTest
            .HttpClient
            .PostAsync("api/v1/accounts/login", integrationTest.Serialize(command));

        var tokenDto = await integrationTest.Deserialize<JwtTokenDto>(response.Content);
        
        integrationTest.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", tokenDto!.Token);
    }
    
    public static int CountAll<TEntity>(this IntegrationTest integrationTest) where TEntity : class
    {
        var sp = integrationTest.Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = sp!.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        return context!.Set<TEntity>().Count();
    }

    public static List<TEntity> GetAll<TEntity>(this IntegrationTest integrationTest) where TEntity : class
    {
        var sp = integrationTest.Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = sp!.CreateScope();
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        return context!.Set<TEntity>().ToList();
    }
}