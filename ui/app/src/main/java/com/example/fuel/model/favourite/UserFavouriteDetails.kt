package com.example.fuel.model.favourite

import java.util.Date

data class UserFavouriteDetails(
    val fuelStationId: Long,
    val username: String,
    val city: String,
    val street: String?,
    val stationChain: String,
    val latitude: Double,
    val longitude: Double,
    val createdAt: Date
) {

    fun address(): String = city + if (street != null) ", $street" else ""
}
