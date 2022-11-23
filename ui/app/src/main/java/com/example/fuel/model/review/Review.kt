package com.example.fuel.model.review

import java.util.Date

data class Review(
    val id: Long,
    val rate: Int,
    val content: String?,
    val username: String,
    val userId: Long,
    val fuelStationId: Long,
    val createdAt: Date,
    val updatedAt: Date)