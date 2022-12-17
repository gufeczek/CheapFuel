using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.FuelPrices.Commands.UpdateFuelPriceByOwner;
using Application.Models.FuelPriceDtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.UnitTests.FuelPrices.Commands.UpdateFuelPriceByOwner;

public class UpdateFuelPriceByOwnerCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IFuelStationRepository> _fuelStationRepository;
    private readonly Mock<IOwnedStationRepository> _ownedStationRepository;
    private readonly Mock<IFuelPriceRepository> _fuelPriceRepository;
    private readonly Mock<IFuelAtStationRepository> _fuelAtStationRepository;
    private readonly Mock<IUserPrincipalService> _userPrincipalService;
    private readonly Mock<IMapper> _mapper;
    private readonly UpdateFuelPriceByOwnerCommandHandler _handler;

    public UpdateFuelPriceByOwnerCommandHandlerTest()
    {
        _userRepository = new Mock<IUserRepository>();
        _fuelStationRepository = new Mock<IFuelStationRepository>();
        _ownedStationRepository = new Mock<IOwnedStationRepository>();
        _fuelPriceRepository = new Mock<IFuelPriceRepository>();
        _fuelAtStationRepository = new Mock<IFuelAtStationRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork
            .Setup(x => x.Users)
            .Returns(_userRepository.Object);
        _unitOfWork
            .Setup(x => x.FuelStations)
            .Returns(_fuelStationRepository.Object);
        _unitOfWork
            .Setup(x => x.OwnedStations)
            .Returns(_ownedStationRepository.Object);
        _unitOfWork
            .Setup(x => x.FuelPrices)
            .Returns(_fuelPriceRepository.Object);
        _unitOfWork
            .Setup(x => x.FuelsAtStation)
            .Returns(_fuelAtStationRepository.Object);
        _userPrincipalService = new Mock<IUserPrincipalService>();
        _mapper = new Mock<IMapper>();

        _handler = new UpdateFuelPriceByOwnerCommandHandler(_unitOfWork.Object, _userPrincipalService.Object, _mapper.Object);
    }

    [Fact]
    public async Task Creates_new_fuel_prices_for_fuel_station_when_performed_by_owner()
    {
        // Arrange
        const string username = "Owner";
        const long userId = 1;
        const long fuelStationId = 1;
        var user = new User { Id = userId, Username = username };
        var fuelStation = new FuelStation { Id = fuelStationId };
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync(fuelStation);

        _ownedStationRepository
            .Setup(x => x.ExistsByUserIdAndFuelStationIdAsync(userId, fuelStationId))
            .ReturnsAsync(true);

        _fuelAtStationRepository
            .Setup(x => x.CountAllByFuelStationIdAndFuelTypesIdsAsync(fuelStationId, new List<long> { 1, 2 }))
            .ReturnsAsync(2);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Creates_new_fuel_prices_for_fuel_station_when_performed_by_admin()
    {
        // Arrange
        const string username = "Admin";
        const long userId = 1;
        const long fuelStationId = 1;
        var user = new User { Id = userId, Username = username, Role = Role.Admin};
        var fuelStation = new FuelStation { Id = fuelStationId };
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);

        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync(fuelStation);

        _ownedStationRepository
            .Setup(x => x.ExistsByUserIdAndFuelStationIdAsync(userId, fuelStationId))
            .ReturnsAsync(false);

        _fuelAtStationRepository
            .Setup(x => x.CountAllByFuelStationIdAndFuelTypesIdsAsync(fuelStationId, new List<long> { 1, 2 }))
            .ReturnsAsync(2);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_user_is_not_logged_in()
    {
        // Arrange
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns((string)null!);

        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedException>()
            .WithMessage("User is not logged in!");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_user_is_not_found()
    {
        // Arrange
        const string username = "User";
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);

        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"User not found for username = {username}");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_fuel_station_not_found()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(new User());
        
        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync((FuelStation)null!);

        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {dto.FuelStationId}");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_given_duplicate_fuel_prices()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var dto = CreateDto();
        dto.FuelPrices![1].FuelTypeId = 1;
        
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(new User());
        
        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync(new FuelStation());

        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Request should not contains duplicate fuel types!");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_fuel_station_does_not_have_given_fuel_type()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(new User());
        
        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync(new FuelStation());

        _fuelAtStationRepository
            .Setup(x => x.CountAllByFuelStationIdAndFuelTypesIdsAsync(It.IsAny<long>(), It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(1);
        
        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Some of given fuel types are not in the fuel station");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Fail_to_create_new_fuel_prices_if_fuel_station_does_not_belong_to_logged_user()
    {
        // Arrange
        const string username = "User";
        const long fuelStationId = 1L;
        
        var dto = CreateDto();
        var command = new UpdateFuelPriceByOwnerCommand(dto);

        _userPrincipalService
            .Setup(x => x.GetUserName())
            .Returns(username);
        
        _userRepository
            .Setup(x => x.GetByUsernameAsync(username))
            .ReturnsAsync(new User());
        
        _fuelStationRepository
            .Setup(x => x.GetFuelStationWithFuelTypesAsync(fuelStationId))
            .ReturnsAsync((FuelStation)null!);

        _fuelAtStationRepository
            .Setup(x => x.CountAllByFuelStationIdAndFuelTypesIdsAsync(It.IsAny<long>(), It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(2);

        _ownedStationRepository
            .Setup(x => x.ExistsByUserIdAndFuelStationIdAsync(It.IsAny<long>(), It.IsAny<long>()))
            .ReturnsAsync(false);
        
        // Act
        Func<Task<IEnumerable<FuelPriceDto>>> act = _handler.Awaiting(x => x.Handle(command, CancellationToken.None));
        
        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Fuel station not found for id = {dto.FuelStationId}");
        
        _fuelPriceRepository.Verify(x => x.AddAll(It.IsAny<IEnumerable<FuelPrice>>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveAsync(), Times.Never);
    }

    private NewFuelPricesAtStationDto CreateDto() => new()
    {
        FuelStationId = 1,
        FuelPrices = new List<NewFuelPriceDto>
        {
            new()
            {
                Available = true,
                Price = 2.0M,
                FuelTypeId = 1
            },
            new()
            {
                Available = true,
                Price = 3.0M,
                FuelTypeId = 2
            }
        }
    };
}