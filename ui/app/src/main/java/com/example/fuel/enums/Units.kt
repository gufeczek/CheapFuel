package com.example.fuel.enums

enum class Units(val fromMeters: Double, val abbr: String) {
    KM(1000.0, "km"),
    MILE(1609.344, "mile")
}