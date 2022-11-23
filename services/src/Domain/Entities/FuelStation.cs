using Domain.Common;

namespace Domain.Entities;

public class FuelStation : AuditableEntity
{
    public string? Name { get; set; }
    public Address? Address { get; set; }
    public GeographicalCoordinates? GeographicalCoordinates { get; set; }

    public long StationChainId { get; set; }
    public StationChain? StationChain { get; set; }

    private List<OpeningClosingTime> _openingClosingTimes = new();
    public IReadOnlyCollection<OpeningClosingTime> OpeningClosingTimes => _openingClosingTimes.AsReadOnly();
    
    private List<FuelPrice> _fuelPrices = new();
    public IReadOnlyCollection<FuelPrice> FuelPrices => _fuelPrices;

    private List<FuelAtStation> _fuelTypes = new();
    public IReadOnlyCollection<FuelAtStation> FuelTypes => _fuelTypes;

    private List<ServiceAtStation> _services = new();
    public IReadOnlyCollection<ServiceAtStation> ServiceAtStations => _services;
}