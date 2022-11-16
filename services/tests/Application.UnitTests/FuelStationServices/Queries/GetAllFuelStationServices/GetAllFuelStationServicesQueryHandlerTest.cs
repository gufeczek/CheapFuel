using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelStationServices.Queries.GetAllFuelStationServices;
using Application.Models;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Common.Pagination.Request;
using Domain.Common.Pagination.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesQueryHandlerTest
{
    private readonly Mock<IFuelStationServiceRepository> _serviceRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetAllFuelStationServicesQueryHandler _handler;

    public GetAllFuelStationServicesQueryHandlerTest()
    {
        _serviceRepository = new Mock<IFuelStationServiceRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.Services)
            .Returns(_serviceRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetAllFuelStationServicesQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_page_of_fuel_station_services()
    {
        // Arrange
        var pageRequestDto = new PageRequestDto { PageSize = 1, PageNumber = 10, Sort = null };
        var pageRequest = new PageRequest<FuelStationService>
        {
            PageNumber = (int)pageRequestDto.PageNumber, 
            PageSize = (int)pageRequestDto.PageSize, 
            Sort = null
        };
        var query = new GetAllFuelStationServicesQuery(pageRequestDto);
        
        List<FuelStationService> data = CreateData();
        List<FuelStationServiceDto> dataDtos = CreateDto(data);
        Page<FuelStationService> services = CreatePage(pageRequest, data);
        
        _serviceRepository
            .Setup(x => x.GetAllAsync(It.IsAny<PageRequest<FuelStationService>>()))
            .ReturnsAsync(services);

        _mapper
            .Setup(x => x.Map<IEnumerable<FuelStationServiceDto>>(data))
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
        var query = new GetAllFuelStationServicesQuery(pageRequestDto);
        
        // Act
        Func<Task<Page<FuelStationServiceDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>();
        
        _serviceRepository.Verify(x => x.GetAllAsync(It.IsAny<PageRequest<FuelStationService>>()), Times.Never);
        _mapper.Verify(x => x.Map<IEnumerable<FuelStationServiceDto>>(It.IsAny<IEnumerable<FuelStationService>>()), Times.Never);
    }

    private List<FuelStationService> CreateData() => new()
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

    private List<FuelStationServiceDto> CreateDto(List<FuelStationService> data) => new()
    {
        new() { Id = 1, Name = data[0].Name! },
        new() { Id = 2, Name = data[1].Name! }
    };

    private Page<E> CreatePage<E>(PageRequest<FuelStationService> pageRequest, IEnumerable<E> data) => new()
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