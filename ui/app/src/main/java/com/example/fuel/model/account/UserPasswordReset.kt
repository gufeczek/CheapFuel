package com.example.fuel.model.account

data class UserPasswordReset (
    var email: String,
    var token: String,
    var newPassword: String,
    var confirmNewPassword: String
)
