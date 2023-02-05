package com.example.fuel.model.account

data class ChangePassword(
    val oldPassword: String,
    val newPassword: String,
    val confirmNewPassword: String
)
