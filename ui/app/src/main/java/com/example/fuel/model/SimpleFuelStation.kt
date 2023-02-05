package com.example.fuel.model

import java.util.*

data class SimpleFuelStation(
    val id: Long,
    val stationChainName: String,
    val price: Double,
    val lastFuelPriceUpdate: Date,
    val latitude: Double,
    val longitude: Double) {

    fun parsePrice(): String {
        return String.format("%.2f", price).replace(".", ",") + " zł"
    }
}
