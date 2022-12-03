using System.Collections.Generic;
using Application.FuelPrices.Commands.UpdateFuelPriceByOwner;
using Application.Models.FuelPriceDtos;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelPrices.Commands.UpdateFuelPriceByOwner;

public class UpdateFuelPriceByOwnerCommandValidatorTest
{
    private readonly UpdateFuelPriceByOwnerCommandValidator _validator;

    public UpdateFuelPriceByOwnerCommandValidatorTest()
    {
        _validator = new UpdateFuelPriceByOwnerCommandValidator();
    }

    [Theory]
    [MemberData(nameof(CorrectData))]
    public void Validation_passes_for_correct_data(NewFuelPricesAtStationDto dto)
    {
        // Arrange
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validation_fails_for_null_dto()
    {
        // Arrange 
        var command = new UpdateFuelPriceByOwnerCommand(null);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void Validation_fails_for_fuel_station_id_smaller_than_one(long fuelStationId)
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = fuelStationId, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = 1.0M, 
                    FuelTypeId = 1
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].Available);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].Price);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].FuelTypeId);
    }

    [Fact]
    public void Validation_fails_for_null_fuel_station_id()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = null, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = 1.0M, 
                    FuelTypeId = 1
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].Available);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].Price);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices![0].FuelTypeId);
    }

    [Fact]
    public void Validation_fails_for_null_fuel_price_list()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto { FuelStationId = 1L, FuelPrices = null };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
    }
    
    [Fact]
    public void Validation_fails_for_empty_fuel_price_list()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto { FuelStationId = 1L, FuelPrices = new List<NewFuelPriceDto>() };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
    }

    [Fact]
    public void Validation_fails_for_null_available_value()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = 1L, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = null, 
                    Price = 1.0M, 
                    FuelTypeId = 1
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Available");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Price");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].FuelTypeId");
    }

    [Fact]
    public void Validation_fails_for_null_price_value()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = 1L, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = null, 
                    FuelTypeId = 1
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Available");
        result.ShouldHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Price");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].FuelTypeId");
    }
    
    [Theory]
    [InlineData(-0.1)]
    [InlineData(-1000.0)]
    public void Validation_fails_for_price_value_smaller_than_zero(decimal price)
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = 1L, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = price, 
                    FuelTypeId = 1
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Available");
        result.ShouldHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Price");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].FuelTypeId");
    }

    [Fact]
    public void Validation_fails_for_null_fuel_type_id()
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = 1L, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = 1.0M, 
                    FuelTypeId = null
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Available");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Price");
        result.ShouldHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].FuelTypeId");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999999)]
    public void Validation_fails_for_fuel_type_id_smaller_than_one(long fuelTypeId)
    {
        // Arrange 
        var dto = new NewFuelPricesAtStationDto
        {
            FuelStationId = 1L, 
            FuelPrices = new List<NewFuelPriceDto>
            {
                new()
                {
                    Available = true, 
                    Price = 1.0M, 
                    FuelTypeId = fuelTypeId
                }
            }
        };
        var command = new UpdateFuelPriceByOwnerCommand(dto);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelStationId);
        result.ShouldNotHaveValidationErrorFor(c => c.FuelPricesAtStationDto!.FuelPrices);
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Available");
        result.ShouldNotHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].Price");
        result.ShouldHaveValidationErrorFor("FuelPricesAtStationDto.FuelPrices[0].FuelTypeId");
    }

    public static IEnumerable<object[]> CorrectData => new List<object[]>
    {
        new object[]
        {
            new NewFuelPricesAtStationDto
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
                        FuelTypeId = 1
                    }
                }
            }
        },
        new object[]
        {
            new NewFuelPricesAtStationDto()
            {
                FuelStationId = 999,
                FuelPrices = new List<NewFuelPriceDto>
                {
                    new()
                    {
                        Available = true,
                        Price = 2.0M,
                        FuelTypeId = 1
                    }
                }
            }
        },
        new object[]
        {
            new NewFuelPricesAtStationDto()
            {
                FuelStationId = 1,
                FuelPrices = new List<NewFuelPriceDto>
                {
                    new()
                    {
                        Available = false,
                        Price = 0.0M,
                        FuelTypeId = 999
                    }
                }
            }
        }
    };
}