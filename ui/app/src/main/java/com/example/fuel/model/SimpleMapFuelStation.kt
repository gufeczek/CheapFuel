package com.example.fuel.model

data class SimpleMapFuelStation(
    val id: Long,
    val stationChainName: String,
    val price: Double,
    val latitude: Double,
    val longitude: Double) {

    fun parsePrice(): String {
        return String.format("%.2f", price).replace(".", ",") + " zł"
    }
}
