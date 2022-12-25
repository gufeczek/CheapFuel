package com.example.fuel.model.calculator

data class MostEconomicalFuelStationRequest(
    var userLongitude: Double?,
    var userLatitude: Double?,
    var fuelConsumption: Double?,
    var fuelAmountToBuy: Double?,
    var fuelTypeId: Long?
) {
    constructor() : this(null, null, null, null, null)
}