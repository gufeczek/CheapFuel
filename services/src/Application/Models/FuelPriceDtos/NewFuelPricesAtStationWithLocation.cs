namespace Application.Models.FuelPriceDtos;

public sealed class NewFuelPricesAtStationWithLocation
{
    public long? FuelStationId { get; set; }
    public double? UserLongitude { get; set; }
    public double? UserLatitude { get; set; }    
    public List<NewFuelPriceDto>? FuelPrices { get; set; }
}