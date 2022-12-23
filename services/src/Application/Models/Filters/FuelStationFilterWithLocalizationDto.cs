namespace Application.Models.Filters;

public sealed record FuelStationFilterWithLocalizationDto(
    long FuelTypeId, 
    IEnumerable<long>? ServicesIds, 
    IEnumerable<long>? StationChainsIds, 
    decimal? MinPrice, 
    decimal? MaxPrice,
    double? UserLongitude,
    double? UserLatitude,
    double? Distance);