package com.example.fuel.model

import java.util.Date

data class UserDetails(
    val username: String,
    val email: String,
    val emailConfirmed: Boolean,
    val multiFactorAuthEnabled: Boolean,
    val role: String,
    val status: String,
    val createdAt: Date
)