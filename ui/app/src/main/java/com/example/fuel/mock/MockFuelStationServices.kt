package com.example.fuel.mock

import com.example.fuel.model.FuelStationService

fun getFuelStationServices(): Array<FuelStationService> {
    return arrayOf(
        FuelStationService(1, "Myjnia"),
        FuelStationService(2, "Toalety"),
        FuelStationService(3, "Prysznic"),
        FuelStationService(4, "Restauracja"),
        FuelStationService(5, "Bar/Bistro"),
        FuelStationService(6, "Warsztat"),
        FuelStationService(7, "DocStop")
    )
}