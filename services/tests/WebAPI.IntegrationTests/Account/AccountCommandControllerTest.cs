using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Application.Users.Commands.ChangeUserRole;
using Application.Users.Commands.RegisterUser;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.Account;

public class AccountCommandControllerTest : IntegrationTest
{
    public AccountCommandControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper)
        : base(
            factory, 
            outputHelper, 
            new IPredefinedData[] { new AccountsData() }) { }

    [Fact]
    public async Task Registers_user()
    {
        // Arrange
        var command = new RegisterUserCommand("NewUser", "newuser@email.com", "Password123", "Password123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/register", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<UserDetailsDto>(response.Content);
        createdObject!.Email.Should().Be(command.Email);
        createdObject.Username.Should().Be(command.Username);
        createdObject.Role.Should().Be(Role.User);
        createdObject.Status.Should().Be(AccountStatus.Active);
        
        this.CountAll<User>().Should()
            .Be(AccountsData.InitialUserCount + 1);
    }

    [Fact]
    public async Task Registration_fails_for_invalid_username()
    {
        // Arrange
        var command = new RegisterUserCommand("sm", "newuser@email.com", "Password123", "Password123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/register", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<User>().Should()
            .Be(AccountsData.InitialUserCount);
    }
    
    [Fact]
    public async Task Registration_fails_for_duplicate_username()
    {
        // Arrange
        var command = new RegisterUserCommand(AccountsData.UserUsername, "newuser@email.com", "Password123", "Password123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/register", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<User>().Should()
            .Be(AccountsData.InitialUserCount);
    }

    [Fact]
    public async Task Registration_fails_for_duplicate_email()
    {
        // Arrange
        var command = new RegisterUserCommand("NewUser", AccountsData.UserEmailAddress, "Password123", "Password123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/register", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<User>().Should()
            .Be(AccountsData.InitialUserCount);
    }
    
    [Fact]
    public async Task Registration_fails_for_not_matching_passwords()
    {
        // Arrange
        var command = new RegisterUserCommand("NewUser", "newuser@email.com", "Password123", "Password");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/register", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<User>().Should()
            .Be(AccountsData.InitialUserCount);
    }

    [Fact]
    public async Task Login_in_user()
    {
        // Arrange
        var command = new AuthenticateUserCommand(AccountsData.UserUsername, AccountsData.Password);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/login", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Text.Plain);

        var createdObject = await response.Content.ReadAsStringAsync();
        createdObject.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Login_fails_for_invalid_username()
    {
        // Arrange
        var command = new AuthenticateUserCommand(AccountsData.InvalidUsername, AccountsData.Password);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/login", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Login_fails_for_invalid_password()
    {
        // Arrange
        var command = new AuthenticateUserCommand(AccountsData.UserUsername, AccountsData.InvalidPassword);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/login", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Changes_user_role()
    {
        // Arrange
        await this.AuthorizeAdmin();
        
        var command = new ChangeUserRoleCommand(AccountsData.UserUsername, Role.Owner);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-role", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Role.Should().Be(Role.Owner);
    }

    [Fact]
    public async Task Change_user_role_fails_if_not_logged_in()
    {
        // Arrange
        var command = new ChangeUserRoleCommand(AccountsData.UserUsername, Role.Owner);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-role", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Role.Should().Be(Role.User);
    }
    
    [Fact]
    public async Task Change_user_role_fails_if_perform_by_user()
    {
        // Arrange
        await this.AuthorizeUser();
        
        var command = new ChangeUserRoleCommand(AccountsData.UserUsername, Role.Owner);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-role", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Role.Should().Be(Role.User);
    }
    
    [Fact]
    public async Task Change_user_role_fails_if_perform_by_owner()
    {
        // Arrange
        await this.AuthorizeOwner();
        
        var command = new ChangeUserRoleCommand(AccountsData.UserUsername, Role.Owner);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-role", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Role.Should().Be(Role.User);
    }
}