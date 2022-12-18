namespace Application.Models.Filters;

public sealed record FuelStationFilterDto(
    long FuelTypeId, 
    IEnumerable<long>? ServicesIds, 
    IEnumerable<long>? StationChainsIds, 
    decimal? MinPrice, 
    decimal? MaxPrice);