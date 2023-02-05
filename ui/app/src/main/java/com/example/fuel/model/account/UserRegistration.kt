package com.example.fuel.model.account

data class UserRegistration (
    var username: String,
    var email: String,
    var password: String,
    var confirmPassword: String
)