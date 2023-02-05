package com.example.fuel.model.fuelstation

data class NewFuelStation(
    val name: String,
    val address: Address,
    val geographicalCoordinates: GeographicalCoordinates,
    var stationChainId: Long,
    val fuelTypes: Array<Long>,
    val services: Array<Long>)