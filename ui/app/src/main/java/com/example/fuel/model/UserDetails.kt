package com.example.fuel.model

import java.util.Date

data class UserDetails(
    val username: String,
    val email: String,
    val emailConfirmed: String,
    val multiFactorAuthEnabled: String,
    val role: String,
    val accountStatus: String,
    val createdAt: Date
)