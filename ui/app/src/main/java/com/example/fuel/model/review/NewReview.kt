package com.example.fuel.model.review

data class NewReview(
    val rate: Int,
    val content: String?,
    val fuelStationId: Long)
