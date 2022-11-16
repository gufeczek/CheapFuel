package com.example.fuel.model

import java.time.LocalDateTime

data class Price(
    val price: Double,
    val available: Boolean,
    val createdAt: LocalDateTime
)