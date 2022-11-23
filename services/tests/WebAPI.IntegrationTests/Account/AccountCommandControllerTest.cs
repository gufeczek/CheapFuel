using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.Users.Commands.AuthenticateUser;
using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.ChangeUserRole;
using Application.Users.Commands.GeneratePasswordResetToken;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.ResetPassword;
using Domain.Entities;
using Domain.Entities.Tokens;
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
            new IPredefinedData[] { new AccountsData(), new AccountAdditionalData() }) { }

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
            .Be(AccountsData.InitialUserCount + AccountAdditionalData.InitialUserCount + 1);
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
            .Be(AccountsData.InitialUserCount + AccountAdditionalData.InitialUserCount);
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
            .Be(AccountsData.InitialUserCount  + AccountAdditionalData.InitialUserCount);
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
            .Be(AccountsData.InitialUserCount  + AccountAdditionalData.InitialUserCount);
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
            .Be(AccountsData.InitialUserCount + AccountAdditionalData.InitialUserCount);
    }

    [Fact]
    public async Task Login_in_user()
    {
        // Arrange
        var command = new AuthenticateUserCommand(AccountsData.UserUsername, AccountsData.DefaultPassword);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/login", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await response.Content.ReadAsStringAsync();
        createdObject.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_fails_for_invalid_username()
    {
        // Arrange
        var command = new AuthenticateUserCommand(AccountsData.InvalidUsername, AccountsData.DefaultPassword);
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

    [Fact]
    public async Task Verifies_user_email_address()
    {
        // Arrange
        await this.AuthorizeGenericUser(AccountAdditionalData.UserWithoutConfirmedEmailUsername, AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/accounts/verify/{AccountAdditionalData.ActiveEmailTokenCode}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithoutConfirmedEmailUsername);
        user.EmailConfirmed.Should().BeTrue();

        var tokens = this.GetAll<EmailVerificationToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialEmailTokenCount - 1);

        var oldToken = tokens.FirstOrDefault(t => t.Id == AccountAdditionalData.ActiveEmailTokenId);
        oldToken.Should().BeNull();
    }

    [Fact]
    public async Task Fails_to_verify_user_email_address_if_user_not_logged_in()
    {
        // Act
        var response = await HttpClient.GetAsync($"api/v1/accounts/verify/{AccountAdditionalData.ActiveEmailTokenCode}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithoutConfirmedEmailUsername);
        user.EmailConfirmed.Should().BeFalse();
    }
    
    [Fact]
    public async Task Fails_to_verify_user_email_address_for_invalid_token()
    {
        // Arrange
        await this.AuthorizeGenericUser(
            AccountAdditionalData.UserWithoutConfirmedEmailUsername, 
            AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/accounts/verify/{AccountAdditionalData.InvalidEmailTokenCode}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithoutConfirmedEmailUsername);
        user.EmailConfirmed.Should().BeFalse();

        var token = this.GetAll<EmailVerificationToken>()
            .First(t => t.Id == AccountAdditionalData.ActiveEmailTokenId);
        token.Count.Should().Be(1);
    }

    [Fact]
    public async Task Fails_to_verify_user_email_address_if_token_is_expired()
    {
        // Arrange
        await this.AuthorizeGenericUser(
            AccountAdditionalData.UserWithExpiredEmailConfirmationTokenUsername, 
            AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync($"api/v1/accounts/verify/{AccountAdditionalData.InvalidEmailTokenCode}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithoutConfirmedEmailUsername);
        user.EmailConfirmed.Should().BeFalse();
    }

    [Fact]
    public async Task Generates_email_address_verification_token()
    {
        // Arrange
        await this.AuthorizeGenericUser(
            AccountAdditionalData.UserWithoutEmailConfirmationTokenUsername, 
            AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync("api/v1/accounts/generate-verification-token");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = this.GetAll<EmailVerificationToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialEmailTokenCount + 1);

        var newToken = tokens.FirstOrDefault(t => t.UserId == AccountAdditionalData.UserWithoutEmailConfirmationTokenId);
        newToken.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Generating_email_address_verification_token_removes_old_user_tokens()
    {
        // Arrange
        await this.AuthorizeGenericUser(
            AccountAdditionalData.UserWithExpiredEmailConfirmationTokenUsername, 
            AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync("api/v1/accounts/generate-verification-token");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = this.GetAll<EmailVerificationToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialEmailTokenCount);

        var expiredToken = tokens.FirstOrDefault(t => t.Id == AccountAdditionalData.ExpiredEmailTokenId);
        expiredToken.Should().BeNull();

        var newToken = tokens.FirstOrDefault(t => t.UserId == AccountAdditionalData.UserWithExpiredEmailConfirmationTokenId);
        newToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Fails_to_generates_email_address_verification_token_if_user_is_not_logged_id()
    {
        // Act
        var response = await HttpClient.GetAsync("api/v1/accounts/generate-verification-token");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var tokens = this.GetAll<EmailVerificationToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialEmailTokenCount);
    }

    [Fact]
    public async Task Fails_to_generates_email_address_verification_token_if_user_already_has_verified_email()
    {
        // Arrange
        await this.AuthorizeGenericUser(
            AccountAdditionalData.UserWithVerifiedEmailAddressUsername, 
            AccountsData.DefaultPassword);
        
        // Act
        var response = await HttpClient.GetAsync("api/v1/accounts/generate-verification-token");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var tokens = this.GetAll<EmailVerificationToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialEmailTokenCount);
    }

    [Fact]
    public async Task Changes_user_password()
    {
        // Arrange
        await this.AuthorizeGenericUser(AccountsData.UserUsername, AccountsData.DefaultPassword);

        var command = new ChangePasswordCommand(AccountsData.DefaultPassword, "NewPassword123", "NewPassword123");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Password.Should().NotBe(AccountsData.DefaultPasswordHash);
    }

    [Fact]
    public async Task Fails_to_change_user_password_if_user_is_not_logged_in()
    {
        // Arrange
        var command = new ChangePasswordCommand(AccountsData.DefaultPassword, "NewPassword123", "NewPassword123");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Fails_to_change_user_password_if_old_password_is_invalid()
    {
        // Arrange
        await this.AuthorizeGenericUser(AccountsData.UserUsername, AccountsData.DefaultPassword);

        var command = new ChangePasswordCommand(AccountsData.InvalidPassword, "NewPassword123", "NewPassword123");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Password.Should().Be(AccountsData.DefaultPasswordHash);
    }

    [Fact]
    public async Task Fails_to_change_user_password_if_new_passwords_do_not_match()
    {
        // Arrange
        await this.AuthorizeGenericUser(AccountsData.UserUsername, AccountsData.DefaultPassword);

        var command = new ChangePasswordCommand(AccountsData.DefaultPassword, "NewPassword123", "InvalidNewPassword123");
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Password.Should().Be(AccountsData.DefaultPasswordHash);
    }

    [Fact]
    public async Task Fails_to_change_user_password_if_new_password_is_the_same_as_the_old_one()
    {
        // Arrange
        await this.AuthorizeGenericUser(AccountsData.UserUsername, AccountsData.DefaultPassword);

        var command = new ChangePasswordCommand(AccountsData.DefaultPassword, AccountsData.DefaultPassword, AccountsData.DefaultPassword);
        var body = this.Serialize(command);

        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/change-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountsData.UserUsername);
        user.Password.Should().Be(AccountsData.DefaultPasswordHash);
    }

    [Fact]
    public async Task Generates_password_reset_token()
    {
        // Arrange
        var command = new GeneratePasswordResetTokenCommand(AccountAdditionalData.UserWithoutPasswordResetTokenEmail);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/generate-password-reset-token", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = this.GetAll<PasswordResetToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialPasswordResetTokenCount + 1);

        var token = tokens.FirstOrDefault(t => t.UserId == AccountAdditionalData.UserWithoutPasswordResetTokenId);
        token.Should().NotBeNull();
    }
    
        
    [Fact]
    public async Task Generating_password_reset_token_removes_old_user_tokens()
    {
        // Arrange
        var command = new GeneratePasswordResetTokenCommand(AccountAdditionalData.UserWithPasswordResetTokenEmail);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/generate-password-reset-token", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = this.GetAll<PasswordResetToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialPasswordResetTokenCount);

        var oldToken = tokens.FirstOrDefault(t => t.Id == AccountAdditionalData.ActivePasswordResetTokenId);
        oldToken.Should().BeNull();
        
        var newToken = tokens.FirstOrDefault(t => t.UserId == AccountAdditionalData.UserWithPasswordResetTokenId);
        newToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Fails_to_generate_password_reset_token_if_user_with_given_email_not_exists()
    {
        // Arrange
        var command = new GeneratePasswordResetTokenCommand(AccountsData.InvalidEmail);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/generate-password-reset-token", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = this.GetAll<PasswordResetToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialPasswordResetTokenCount);
    }

    [Fact]
    public async Task Resets_user_password()
    {
        // Arrange
        var command = new ResetPasswordCommand(
            AccountAdditionalData.UserWithPasswordResetTokenEmail,
            AccountAdditionalData.ActivePasswordResetTokenCode, 
            "NewPassword123", 
            "NewPassword123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/reset-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithPasswordResetTokenUsername);
        user.Password.Should().NotBe(AccountsData.DefaultPasswordHash);

        var tokens = this.GetAll<PasswordResetToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialPasswordResetTokenCount - 1);

        var oldToken = tokens.FirstOrDefault(t => t.Id == AccountAdditionalData.ActivePasswordResetTokenId);
        oldToken.Should().BeNull();
    }

    [Fact]
    public async Task Fails_to_reset_user_password_if_user_with_given_email_address_has_not_been_found()
    {
        // Arrange
        var command = new ResetPasswordCommand(
            AccountsData.InvalidEmail,
            AccountAdditionalData.ActivePasswordResetTokenCode, 
            "NewPassword123", 
            "NewPassword123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/reset-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Fails_to_reset_user_password_for_invalid_token()
    {
        // Arrange
        var command = new ResetPasswordCommand(
            AccountAdditionalData.UserWithPasswordResetTokenEmail,
            AccountAdditionalData.InvalidPasswordResetTokenCode, 
            "NewPassword123", 
            "NewPassword123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/reset-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithPasswordResetTokenUsername);
        user.Password.Should().Be(AccountsData.DefaultPasswordHash);

        var tokens = this.GetAll<PasswordResetToken>().ToList();
        tokens.Count.Should().Be(AccountAdditionalData.InitialPasswordResetTokenCount);

        var oldToken = tokens.FirstOrDefault(t => t.Id == AccountAdditionalData.ActivePasswordResetTokenId);
        oldToken.Should().NotBeNull();
        oldToken!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task Fails_to_reset_user_password_if_token_is_expired()
    {
        // Arrange
        var command = new ResetPasswordCommand(
            AccountAdditionalData.UserWithPasswordResetTokenEmail,
            AccountAdditionalData.InvalidPasswordResetTokenCode, 
            "NewPassword123", 
            "NewPassword123");
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/accounts/reset-password", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var user = this.GetAll<User>()
            .First(u => u.Username == AccountAdditionalData.UserWithPasswordResetTokenUsername);
        user.Password.Should().Be(AccountsData.DefaultPasswordHash);
    }
}