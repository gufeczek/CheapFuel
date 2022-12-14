package com.example.fuel.model.price

data class NewFuelPriceResponse(
    val price: Double,
    val available: Boolean,
    val status: String,
    val fuelStationId: Long,
    val fuelTypeId: Long
)