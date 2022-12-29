package com.example.fuel.enums

enum class AccountStatus(val value: String) {
    NEW("Nowe"),
    ACTIVE("Aktywne"),
    BANNED("Zbanowane");

    companion object {
        fun fromString(status: String): AccountStatus {
            return when (status) {
                "New" -> NEW
                "Active" -> ACTIVE
                "Banned" -> BANNED
                else -> { throw IllegalArgumentException() }
            }
        }
    }
}