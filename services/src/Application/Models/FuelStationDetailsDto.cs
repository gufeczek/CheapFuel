namespace Application.Models;

public sealed class FuelStationDetailsDto
{
    public string? Name { get; set; }
    public AddressDto? Address { get; set; }
    public StationChainDto? StationChain { get; set; }
    public IEnumerable<OpeningClosingTimeDto>? OpeningClosingTimes { get; set; }
    public IEnumerable<FuelTypeWithPriceDto>? FuelTypes { get; set; }
    public IEnumerable<FuelStationServiceDto>? Services { get; set; }
}