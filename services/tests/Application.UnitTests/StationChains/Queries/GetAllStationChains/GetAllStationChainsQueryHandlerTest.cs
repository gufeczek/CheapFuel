using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Models;
using Application.Models.Pagination;
using Application.StationChains.Queries.GetAllStationChains;
using AutoMapper;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.StationChains.Queries.GetAllStationChains;

public class GetAllStationChainsQueryHandlerTest
{
    private readonly Mock<IStationChainRepository> _stationChainRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetAllStationChainsQueryHandler _handler;

    public GetAllStationChainsQueryHandlerTest()
    {
        _stationChainRepository = new Mock<IStationChainRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.StationChains)
            .Returns(_stationChainRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetAllStationChainsQueryHandler(unitOfWork.Object, _mapper.Object);
    }
    
    [Fact]
    public async Task Returns_page_of_station_chains()
    {
        // Arrange
        var pageRequestDto = new PageRequestDto { PageSize = 1, PageNumber = 10, Sort = null };
        var pageRequest = new PageRequest<StationChain>
        {
            PageNumber = (int)pageRequestDto.PageNumber, 
            PageSize = (int)pageRequestDto.PageSize, 
            Sort = null
        };
        var query = new GetAllStationChainsQuery(pageRequestDto);
        
        List<StationChain> data = CreateData();
        List<StationChainDto> dataDtos = CreateDto(data);
        Page<StationChain> services = CreatePage(pageRequest, data);
        
        _stationChainRepository
            .Setup(x => x.GetAllAsync(It.IsAny<PageRequest<StationChain>>()))
            .ReturnsAsync(services);

        _mapper
            .Setup(x => x.Map<IEnumerable<StationChainDto>>(data))
            .Returns(dataDtos);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task Throws_exception_for_invalid_sort_column()
    {
        // Arrange
        const string column = "Invalid column";
        
        var pageRequestDto = new PageRequestDto
        {
            PageSize = 1, 
            PageNumber = 10, 
            Sort = new SortDto{ SortBy = column, SortDirection = SortDirection.Asc }
        };
        var query = new GetAllStationChainsQuery(pageRequestDto);
        
        // Act
        Func<Task<Page<StationChainDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>();
        
        _stationChainRepository.Verify(x => x.GetAllAsync(It.IsAny<PageRequest<StationChain>>()), Times.Never);
        _mapper.Verify(x => x.Map<IEnumerable<FuelStationServiceDto>>(It.IsAny<IEnumerable<FuelStationService>>()), Times.Never);
    }
    
    private List<StationChain> CreateData() => new()
    {
        new()
        {
            Name = "Service 1", 
            CreatedAt = new DateTime(2022, 10, 1),
            CreatedBy = 1,
            UpdatedAt = new DateTime(2022, 10, 1),
            UpdatedBy = 1
        },
        new()
        {
            Name = "Service 2", 
            CreatedAt = new DateTime(2022, 10, 2),
            CreatedBy = 1,
            UpdatedAt = new DateTime(2022, 10, 2),
            UpdatedBy = 1
        }
    };

    private List<StationChainDto> CreateDto(List<StationChain> data) => new()
    {
        new(1, data[0].Name!),
        new(2, data[1].Name!)
    };

    private Page<E> CreatePage<E>(PageRequest<StationChain> pageRequest, IEnumerable<E> data) => new()
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