using Domain.Common;

namespace Domain.Entities;

public class FuelStation : PermanentEntity
{
    public string? Name { get; set; }
    public Address? Address { get; set; }
    public GeographicalCoordinates? GeographicalCoordinates { get; set; }
    
    private List<OpeningClosingTime> _openingClosingTimes = new();
    public IReadOnlyCollection<OpeningClosingTime> OpeningClosingTimes => _openingClosingTimes.AsReadOnly();
}