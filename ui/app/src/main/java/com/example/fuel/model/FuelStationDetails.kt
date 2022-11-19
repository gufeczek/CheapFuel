package com.example.fuel.model

data class FuelStationDetails(
    val id: Long,
    val name: String?,
    val address: Address,
    val location: FuelStationLocation,
    val stationChain: StationChain,
    val openingClosingTimes: Array<OpeningClosingTime>,
    val fuelTypes: Array<FuelTypeWithPrice>,
    val services: Array<FuelStationService>
) {

    override fun equals(other: Any?): Boolean {
        if (this === other) return true
        if (javaClass != other?.javaClass) return false

        other as FuelStationDetails

        if (id != other.id) return false
        if (name != other.name) return false
        if (address != other.address) return false
        if (stationChain != other.stationChain) return false
        if (!openingClosingTimes.contentEquals(other.openingClosingTimes)) return false
        if (!fuelTypes.contentEquals(other.fuelTypes)) return false
        if (!services.contentEquals(other.services)) return false

        return true
    }

    override fun hashCode(): Int {
        var result = id.hashCode()
        result = 31 * result + name.hashCode()
        result = 31 * result + address.hashCode()
        result = 31 * result + stationChain.hashCode()
        result = 31 * result + openingClosingTimes.contentHashCode()
        result = 31 * result + fuelTypes.contentHashCode()
        result = 31 * result + services.contentHashCode()
        return result
    }
}