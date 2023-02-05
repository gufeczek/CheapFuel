using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.FuelTypes.Queries.GetAllFuelTypes;
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

namespace Application.UnitTests.FuelTypes.Queries.GetAllFuelTypes;

public class GetAllFuelTypesQueryHandlerTest
{
    private readonly Mock<IFuelTypeRepository> _fuelTypeRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly GetAllFuelTypesQueryHandler _handler;

    public GetAllFuelTypesQueryHandlerTest()
    {
        _fuelTypeRepository = new Mock<IFuelTypeRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(u => u.FuelTypes)
            .Returns(_fuelTypeRepository.Object);
        _mapper = new Mock<IMapper>();

        _handler = new GetAllFuelTypesQueryHandler(unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Returns_page_of_fuel_types()
    {
        // Arrange
        var pageRequestDto = new PageRequestDto { PageSize = 1, PageNumber = 10, Sort = null };
        var pageRequest = new PageRequest<FuelType>
        {
            PageNumber = (int)pageRequestDto.PageNumber, 
            PageSize = (int)pageRequestDto.PageSize, 
            Sort = null
        };
        var query = new GetAllFuelTypesQuery(pageRequestDto);
        
        List<FuelType> data = CreateData();
        List<FuelTypeDto> dataDtos = CreateDto(data);
        Page<FuelType> services = CreatePage(pageRequest, data);
        
        _fuelTypeRepository
            .Setup(x => x.GetAllAsync(It.IsAny<PageRequest<FuelType>>()))
            .ReturnsAsync(services);

        _mapper
            .Setup(x => x.Map<IEnumerable<FuelTypeDto>>(data))
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
        var query = new GetAllFuelTypesQuery(pageRequestDto);
        
        // Act
        Func<Task<Page<FuelTypeDto>>> act = _handler.Awaiting(x => x.Handle(query, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>();
        
        _fuelTypeRepository.Verify(x => x.GetAllAsync(It.IsAny<PageRequest<FuelType>>()), Times.Never);
        _mapper.Verify(x => x.Map<IEnumerable<FuelTypeDto>>(It.IsAny<IEnumerable<FuelType>>()), Times.Never);
    }
    
    private List<FuelType> CreateData() => new()
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

    private List<FuelTypeDto> CreateDto(List<FuelType> data) => new()
    {
        new(1, data[0].Name!),
        new(2, data[1].Name!)
    };

    private Page<E> CreatePage<E>(PageRequest<FuelType> pageRequest, IEnumerable<E> data) => new()
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