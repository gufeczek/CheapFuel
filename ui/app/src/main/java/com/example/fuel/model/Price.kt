package com.example.fuel.model

import java.util.Date

data class Price(
    val price: Double,
    val available: Boolean,
    val createdAt: Date
)