package com.example.fuel.enums

enum class Role(val value: String) {
    USER("Użytkownik"),
    OWNER("Właściciel"),
    ADMIN("Admin");

    companion object {
        fun fromString(role: String): Role {
            return when(role) {
                "User" -> USER
                "Owner" -> OWNER
                "Admin" -> ADMIN
                else -> { throw IllegalArgumentException() }
            }
        }
    }
}