namespace Application.Models;

public sealed class FuelStationDetailsDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public AddressDto? Address { get; set; }
    public FuelStationLocationDto Location { get; set; }
    public StationChainDto? StationChain { get; set; }
    public IEnumerable<OpeningClosingTimeDto>? OpeningClosingTimes { get; set; }
    public IEnumerable<FuelTypeWithPriceDto>? FuelTypes { get; set; }
    public IEnumerable<FuelStationServiceDto>? Services { get; set; }
    public IEnumerable<string>? Owners { get; set; }
}