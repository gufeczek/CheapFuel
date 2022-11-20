package com.example.fuel.model

import java.util.Date

data class Review(
    val rate: Int,
    val content: String?,
    val username: String,
    val userId: Long,
    val createdAt: Date,
    val updatedAt: Date)