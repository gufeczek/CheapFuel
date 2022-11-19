package com.example.fuel.model

import java.time.LocalDateTime

data class Comment(
    val rate: Int,
    val content: String?,
    val username: User,
    val userId: Long,
    val createdAt: LocalDateTime,
    val updatedAt: LocalDateTime)