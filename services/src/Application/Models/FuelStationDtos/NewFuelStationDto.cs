namespace Application.Models.FuelStationDtos;

public sealed class NewFuelStationDto
{
    public string? Name { get; set; }
    public NewAddressDto? Address { get; set; }
    public NewGeographicalCoordinatesDto? GeographicalCoordinates { get; set; }
    public long StationChainId { get; set; }
    public IEnumerable<long>? FuelTypes { get; set; }
    public IEnumerable<long>? Services { get; set; }
}