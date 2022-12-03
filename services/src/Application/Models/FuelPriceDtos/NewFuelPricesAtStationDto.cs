namespace Application.Models.FuelPriceDtos;

public sealed class NewFuelPricesAtStationDto
{
    public long? FuelStationId { get; set; }
    public List<NewFuelPriceDto>? FuelPrices { get; set; }
}