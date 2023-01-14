package com.example.fuel.model.report

import java.time.LocalDateTime

data class Report(
    val reviewId: Long,
    val userId: Long,
    val reportStatus: String,
    val createdAt: LocalDateTime?,
    val updatedAt: LocalDateTime?
)
