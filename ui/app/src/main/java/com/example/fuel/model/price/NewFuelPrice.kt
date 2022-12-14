package com.example.fuel.model.price

data class NewFuelPrice(
    val fuelTypeId: Long,
    var price: Double,
    var available: Boolean
)
