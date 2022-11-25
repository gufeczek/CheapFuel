using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Models;
using Application.Models.Pagination;
using Application.Reviews.Queries.GetAllFuelStationReviews;
using AutoMapper;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.Reviews.Queries.GetAllFuelStationReviews;

public class GetAllFuelStationReviewsQueryHandlerTest
{
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IReviewRepository> _reviewRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetAllFuelStationReviewsQueryHandler _handler;

    public GetAllFuelStationReviewsQueryHandlerTest()
    {
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _reviewRepository = new Mock<IReviewRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.FuelStations)
            .Returns(_fuelStationRepository.Object);
        unitOfWork
            .Setup(u => u.Reviews)
            .Returns(_reviewRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetAllFuelStationReviewsQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_page_of_reviews()
    {
        // Arrange
        const long fuelStationId = 1L;
        
        var pageRequestDto = new PageRequestDto { PageSize = 1, PageNumber = 10, Sort = null };
        var pageRequest = new PageRequest<Review>
        {
            PageNumber = (int)pageRequestDto.PageNumber, 
            PageSize = (int)pageRequestDto.PageSize, 
            Sort = null
        };
        var query = new GetAllFuelStationReviewsQuery(fuelStationId, pageRequestDto);

        List<Review> data = CreateData();
        List<FuelStationReviewDto> dataDtos = CreateDto(data);
        Page<Review> reviews = CreatePage(pageRequest, data);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);

        _reviewRepository
            .Setup(x => x.GetAllForFuelStationAsync(fuelStationId, It.IsAny<PageRequest<Review>>()))
            .ReturnsAsync(reviews);

        _mapper
            .Setup(x => x.Map<IEnumerable<FuelStationReviewDto>>(reviews.Data))
            .Returns(dataDtos);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task Throws_exception_for_invalid_fuel_station()
    {
        // Arrange
        const long fuelStationId = 1L;
        
        var pageRequestDto = new PageRequestDto { PageSize = 1, PageNumber = 10, Sort = null };
        var query = new GetAllFuelStationReviewsQuery(fuelStationId, pageRequestDto);

        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<Page<FuelStationReviewDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>();
        
        _reviewRepository.Verify(x => x.GetAllForFuelStationAsync(It.IsAny<long>(), It.IsAny<PageRequest<Review>>()), Times.Never);
    }

    [Fact]
    public async Task Throws_exception_for_invalid_sort_column()
    {
        // Arrange
        const long fuelStationId = 1L;
        const string column = "Invalid column";
        
        var pageRequestDto = new PageRequestDto
        {
            PageSize = 1, 
            PageNumber = 10, 
            Sort = new SortDto{ SortBy = column, SortDirection = SortDirection.Asc }
        };
        var query = new GetAllFuelStationReviewsQuery(1L, pageRequestDto);
        
        _fuelStationRepository
            .Setup(x => x.ExistsById(fuelStationId))
            .ReturnsAsync(true);
        
        // Act
        Func<Task<Page<FuelStationReviewDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>();
        
        _reviewRepository.Verify(x => x.GetAllForFuelStationAsync(It.IsAny<long>(), It.IsAny<PageRequest<Review>>()), Times.Never);
    }

    private List<Review> CreateData() => new()
    {
        new()
        {
            Id = 1,
            Rate = 5,
            Content = "Lorem ipsum",
            UserId = 1,
            FuelStationId = 1,
            CreatedAt = new DateTime(2022, 10, 1),
            CreatedBy = 1,
            UpdatedAt = new DateTime(2022, 10, 1),
            UpdatedBy = 1
        },
        new()
        {
            Id = 2,
            Rate = 3,
            Content = null,
            UserId = 2,
            FuelStationId = 1,
            CreatedAt = new DateTime(2022, 10, 2),
            CreatedBy = 2,
            UpdatedAt = new DateTime(2022, 10, 2),
            UpdatedBy = 2
        }
    };

    private List<FuelStationReviewDto> CreateDto(List<Review> data) => new()
    {
        new()
        {
            Id = data[0].Id,
            Rate = (int)data[0].Rate!,
            Content = data[0].Content,
            Username = "User 1",
            UserId = data[0].UserId,
            CreatedAt = data[0].CreatedAt,
            UpdatedAt = data[0].UpdatedAt
        },
        new()
        {
            Id = data[1].Id,
            Rate = (int)data[1].Rate!,
            Content = data[1].Content,
            Username = "User 2",
            UserId = data[1].UserId,
            CreatedAt = data[1].CreatedAt,
            UpdatedAt = data[1].UpdatedAt
        }
    };

    private Page<TE> CreatePage<TE>(PageRequest<Review> pageRequest, IEnumerable<TE> data) => new()
    {
        PageNumber = pageRequest.PageNumber,
        PageSize = pageRequest.PageSize,
        NextPage = null,
        PreviousPage = null,
        FirstPage = 1,
        LastPage = 1,
        TotalPages = 1,
        TotalElements = 2,
        Sort = null,
        Data = data
    };
}