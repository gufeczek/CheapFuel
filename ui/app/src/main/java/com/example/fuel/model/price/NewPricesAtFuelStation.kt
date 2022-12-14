package com.example.fuel.model.price

data class NewPricesAtFuelStation(
    val fuelStationId: Long,
    val fuelPrices: Array<NewFuelPrice>
)
