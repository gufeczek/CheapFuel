package com.example.fuel.model.favourite

import java.util.Date

data class UserFavourite(
    val fuelStationId: Long,
    val username: String,
    val createdAt: Date
)