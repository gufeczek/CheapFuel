package com.example.fuel.model.price

data class NewPriceAtFuelStationWithLocation(
    val fuelStationId: Long,
    val userLongitude: Double,
    val userLatitude: Double,
    val fuelPrices: Array<NewFuelPrice>
)