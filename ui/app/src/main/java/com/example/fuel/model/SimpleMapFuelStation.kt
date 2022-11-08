package com.example.fuel.model

data class SimpleMapFuelStation(
    val stationChainName: String,
    val price: String,
    val latitude: Double,
    val longitude: Double) {

    fun parsePrice(): String {
        return price.replace(".", ",") + " zł"
    }
}
