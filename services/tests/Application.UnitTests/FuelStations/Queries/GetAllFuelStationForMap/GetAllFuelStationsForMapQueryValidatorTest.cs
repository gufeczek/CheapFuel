using System.Collections.Generic;
using Application.FuelStations.Queries.GetAllFuelStationForMap;
using Application.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.FuelStations.Queries.GetAllFuelStationForMap;

public class GetAllFuelStationsForMapQueryValidatorTest
{
    private readonly GetAllFuelStationsForMapQueryValidator _validator;

    public GetAllFuelStationsForMapQueryValidatorTest()
    {
        _validator = new GetAllFuelStationsForMapQueryValidator();
    }

    [Theory]
    [MemberData(nameof(CorrectData))]
    public void validation_passes_for_correct_data(FuelStationFilterDto filterDto)
    {
        // Arrange
        var query = new GetAllFuelStationsForMapQuery(filterDto);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }

    [Fact]
    public void validation_fails_for_null_filter()
    {
        // Arrange
        var query = new GetAllFuelStationsForMapQuery(null!);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(-10)]
    [InlineData(0)]
    public void validation_fails_for_invalid_id(long id)
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: id,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 1.0M,
            MaxPrice: 2.0M);
        
        var query = new GetAllFuelStationsForMapQuery(filter);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }

    [Fact]
    public void validation_fails_for_negative_min_price()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: -1.0M,
            MaxPrice: 2.0M);
        
        var query = new GetAllFuelStationsForMapQuery(filter);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }
    
    [Fact]
    public void validation_fails_for_negative_max_price()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: null,
            MaxPrice: -2.0M);
        
        var query = new GetAllFuelStationsForMapQuery(filter);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }

    [Fact]
    public void validation_fails_if_min_price_is_greater_than_max_price()
    {
        // Arrange
        var filter = new FuelStationFilterDto(
            FuelTypeId: 1,
            ServicesIds: new List<long> { 1, 2 },
            StationChainsIds: new List<long> { 1, 2 },
            MinPrice: 5.0M,
            MaxPrice: 2.0M);
        
        var query = new GetAllFuelStationsForMapQuery(filter);
        
        // Act
        var result = _validator.TestValidate(query);
        
        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.FuelTypeId);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.ServicesIds);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.StationChainsIds);
        result.ShouldHaveValidationErrorFor(q => q.FilterDto!.MinPrice);
        result.ShouldNotHaveValidationErrorFor(q => q.FilterDto!.MaxPrice);
    }

    public static IEnumerable<object[]> CorrectData => new List<object[]>
    {
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: new List<long> { 1, 2 },
                StationChainsIds: new List<long> { 1, 2 },
                MinPrice: 1.0M,
                MaxPrice: 2.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 10,
                ServicesIds: new List<long>(),
                StationChainsIds: new List<long>(),
                MinPrice: 1.0M,
                MaxPrice: 2.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: null,
                StationChainsIds: new List<long> { 1, 2 },
                MinPrice: 1.0M,
                MaxPrice: 2.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: new List<long> { 1, 2 },
                StationChainsIds: null,
                MinPrice: 1.0M,
                MaxPrice: 2.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: new List<long> { 1, 2 },
                StationChainsIds: new List<long> { 1, 2 },
                MinPrice: 1.0M,
                MaxPrice: 1.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: new List<long> { 1, 2 },
                StationChainsIds: new List<long> { 1, 2 },
                MinPrice: null,
                MaxPrice: 1.0M)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: new List<long> { 1, 2 },
                StationChainsIds: new List<long> { 1, 2 },
                MinPrice: 1.0M,
                MaxPrice: null)
        },
        new object[]
        {
            new FuelStationFilterDto(
                FuelTypeId: 1,
                ServicesIds: null,
                StationChainsIds: null,
                MinPrice: null,
                MaxPrice: null)
        },
    };
}