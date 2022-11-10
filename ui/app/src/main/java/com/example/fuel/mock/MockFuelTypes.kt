package com.example.fuel.mock

import com.example.fuel.model.FuelType

fun getFuelTypes(): Array<FuelType> {
    return arrayOf(
        FuelType(1, "Pb 95", "Benzyna 95"),
        FuelType(2, "Pb 98", "Benzyna 98"),
        FuelType(3, "ON", "Diesel"),
        FuelType(4, "", "Etanol"),
        FuelType(5, "LPG", "LPG"),
        FuelType(6, "", "CNG"),
        FuelType(7, "", "Elektryczny"),
    )
}