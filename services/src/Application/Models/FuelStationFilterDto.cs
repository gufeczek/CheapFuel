namespace Application.Models;

public sealed record FuelStationFilterDto(
    long FuelTypeId, 
    IEnumerable<long>? ServicesIds, 
    IEnumerable<long>? StationChainsIds, 
    decimal? MinPrice, 
    decimal? MaxPrice);