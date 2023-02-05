using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Models;
using Application.Reviews.Commands.CreateReview;
using FluentAssertions;
using WebAPI.IntegrationTests.PredefinedData;
using WebAPI.IntegrationTests.TestConfiguration;
using Xunit;
using Xunit.Abstractions;

namespace WebAPI.IntegrationTests.Review;

public class ReviewCommandControllerTest : IntegrationTest
{
    public ReviewCommandControllerTest(
        TestingWebApiFactory<Program> factory, 
        ITestOutputHelper outputHelper) 
        : base(
            factory, 
            outputHelper,
            new IPredefinedData[] { new AccountsData(), new ReviewCommandControllerData() }) { }

    [Fact]
    public async Task Creates_new_review()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithoutReviewsUsername, AccountsData.DefaultPassword);

        var command = new CreateReviewCommand(5, null, ReviewCommandControllerData.FuelStation1Id);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/reviews", body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<FuelStationReviewDto>(response.Content);
        createdObject!.Id.Should().NotBe(null);
        createdObject.Rate.Should().Be(5);
        createdObject.Content.Should().BeNull();
        createdObject.Username.Should().Be(ReviewCommandControllerData.UserWithoutReviewsUsername);
        createdObject.UserId.Should().Be(ReviewCommandControllerData.UserWithoutReviewsId);
        createdObject.CreatedAt.Should().NotBe(null);
        createdObject.UpdatedAt.Should().BeSameDateAs(createdObject.CreatedAt);

        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount + 1);

        this.GetAll<Domain.Entities.Review>().Where(r => r.UserId == ReviewCommandControllerData.UserWithoutReviewsId)
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task Fails_to_create_review_for_not_logged_user()
    {
        // Arrange
        var command = new CreateReviewCommand(5, null, ReviewCommandControllerData.FuelStation1Id);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/reviews", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }

    [Fact]
    public async Task Fails_to_create_review_if_user_already_rated_fuel_station()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithTwoReviewUsername, AccountsData.DefaultPassword);

        var command = new CreateReviewCommand(5, null, ReviewCommandControllerData.FuelStation1Id);
        var body = this.Serialize(command);
        
        // Act
        var response = await HttpClient.PostAsync("api/v1/reviews", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }

    [Fact]
    public async Task Updates_review_if_performed_by_review_author()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithTwoReviewUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        var dto = new UpdateReviewDto { Content = "New content", Rate = 3 };
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PutAsync($"api/v1/reviews/{reviewId}", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<FuelStationReviewDto>(response.Content);
        createdObject!.Id.Should().Be(reviewId);
        createdObject.Rate.Should().Be(dto.Rate);
        createdObject.Content.Should().Be(dto.Content);
        createdObject.Username.Should().Be(ReviewCommandControllerData.UserWithTwoReviewUsername);
        createdObject.UserId.Should().Be(ReviewCommandControllerData.UserWithTwoReviewId);
        createdObject.CreatedAt.Should().NotBe(null);
        createdObject.UpdatedAt.Should().BeAfter(createdObject.CreatedAt);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }
    
    [Fact]
    public async Task Updates_review_if_performed_by_admin()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithTwoReviewUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        var dto = new UpdateReviewDto { Content = "New content", Rate = 3 };
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PutAsync($"api/v1/reviews/{reviewId}", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);

        var createdObject = await this.Deserialize<FuelStationReviewDto>(response.Content);
        createdObject!.Id.Should().Be(reviewId);
        createdObject.Rate.Should().Be(dto.Rate);
        createdObject.Content.Should().Be(dto.Content);
        createdObject.Username.Should().Be(ReviewCommandControllerData.UserWithTwoReviewUsername);
        createdObject.UserId.Should().Be(ReviewCommandControllerData.UserWithTwoReviewId);
        createdObject.CreatedAt.Should().NotBe(null);
        createdObject.UpdatedAt.Should().BeAfter(createdObject.CreatedAt);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_update_review_if_performed_by_not_logged_user()
    {
        // Arrange
        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        var dto = new UpdateReviewDto { Content = "New content", Rate = 3 };
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PutAsync($"api/v1/reviews/{reviewId}", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_update_review_if_performed_by_not_admin_and_not_review_author()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithoutReviewsUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        var dto = new UpdateReviewDto { Content = "New content", Rate = 3 };
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PutAsync($"api/v1/reviews/{reviewId}", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }
    
    [Fact]
    public async Task Fails_to_update_review_if_review_not_exists()
    {
        // Arrange
        await this.AuthorizeAdmin();
        
        const long reviewId = ReviewCommandControllerData.InvalidReviewId;
        
        var dto = new UpdateReviewDto { Content = "New content", Rate = 3 };
        var body = this.Serialize(dto);
        
        // Act
        var response = await HttpClient.PutAsync($"api/v1/reviews/{reviewId}", body);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
    }

    [Fact]
    public async Task Deletes_existing_review_if_performed_by_review_author()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithTwoReviewUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/reviews/{reviewId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount - 1);
        this.GetAll<Domain.Entities.Review>().Where(r => r.Id == reviewId)
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Deletes_existing_reviews_if_performed_by_admin()
    {
        // Arrange
        await this.AuthorizeAdmin();
        
        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/reviews/{reviewId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount - 1);
        this.GetAll<Domain.Entities.Review>().Where(r => r.Id == reviewId)
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task Fails_to_delete_review_if_performed_by_not_logged_user()
    {
        // Arrange
        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/reviews/{reviewId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_delete_review_if_performed_by_not_admin_and_not_review_author()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithoutReviewsUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.Review1Id;
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/reviews/{reviewId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }
    
    [Fact]
    public async Task Fails_to_delete_review_if_review_not_exists()
    {
        // Arrange
        await this.AuthorizeGenericUser(ReviewCommandControllerData.UserWithoutReviewsUsername, AccountsData.DefaultPassword);

        const long reviewId = ReviewCommandControllerData.InvalidReviewId;
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/v1/reviews/{reviewId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Json);
        
        this.CountAll<Domain.Entities.Review>().Should()
            .Be(ReviewCommandControllerData.ReviewInitialCount);
    }
}