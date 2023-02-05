package com.example.fuel.utils.converters

import com.example.fuel.enums.DecimalSeparator
import com.example.fuel.enums.Units

// TODO: Should support converting meters to multiple different units
object UnitConverter {
    val decimalSeparator = DecimalSeparator.COMMA
    val targetUnit = Units.KM

    fun fromMetersToTarget(meters: Double): String {
        var result = meters / targetUnit.fromMeters

        return if (result > 0) String.format("%.1f km", result)
        else String.format("%.0f m", meters)
    }
}