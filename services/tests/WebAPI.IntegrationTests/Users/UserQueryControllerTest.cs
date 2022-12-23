using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Filters;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.Users;

public class UserQueryControllerTest : IntegrationTest
{
    public UserQueryControllerTest(
        TestingWebApiFactory<Program> factory,
        ITestOutputHelper outputHelper)
        : base(
            factory,
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new UserQueryControllerData() }) { }

    [Fact]
    public async Task Return_page_of_users()
    {
        // Arrange
        await this.AuthorizeAdmin();

        var filter = new UserFilterDto(null, null, null);
        var body = this.Serialize(filter);
        
        const int pageNumber = 1;
        const int pageSize = 10;
        const string sortBy = nameof(User.Username);
        const SortDirection direction = SortDirection.Asc;
        
        
        // Act
        var response = await HttpClient.PostAsync(
            "api/v1/users?" +
            $"PageNumber={pageNumber}&" +
            $"PageSize={pageSize}&" +
            $"Sort.SortBy={sortBy}&" +
            $"Sort.SortDirection={direction}",
            body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var page = await this.Deserialize<Page<UserDetailsDto>>(response.Content);
        page!.PageNumber.Should().Be(pageNumber);
        page.PageSize.Should().Be(pageSize);
        page.TotalElements.Should().Be(UserQueryControllerData.InitialUsersCount);
        page.Data.Should().NotBeNull().And.HaveCount(UserQueryControllerData.InitialUsersCount);
    }

    [Fact]
    public async Task Show_info_fail_for_invalid_username()
    {
        // Arrange
        const string username = "michalek";
        
        // Act 
        var response = await HttpClient.GetAsync($"api/v1/users/{username}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
}