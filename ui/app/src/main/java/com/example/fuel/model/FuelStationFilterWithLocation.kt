package com.example.fuel.model

data class FuelStationFilterWithLocation(
    var fuelTypeId: Long,
    var servicesIds: MutableList<Long>?,
    var stationChainsIds: MutableList<Long>?,
    var minPrice: Double?,
    var maxPrice: Double?,
    var userLongitude: Double?,
    var userLatitude: Double?,
    var distance: Double?
) {

    fun copy(): FuelStationFilterWithLocation {
        return FuelStationFilterWithLocation(
            this.fuelTypeId,
            servicesIds?.toMutableList(),
            stationChainsIds?.toMutableList(),
            minPrice,
            maxPrice,
            userLongitude,
            userLatitude,
            distance
        )
    }

    override fun equals(other: Any?): Boolean {
        if (this === other) return true
        if (javaClass != other?.javaClass) return false

        other as FuelStationFilterWithLocation

        if (fuelTypeId != other.fuelTypeId) return false
        if (servicesIds != other.servicesIds) return false
        if (stationChainsIds != other.stationChainsIds) return false
        if (minPrice != other.minPrice) return false
        if (maxPrice != other.maxPrice) return false
        if (userLongitude != other.userLongitude) return false
        if (userLatitude != other.userLatitude) return false
        if (distance != other.distance) return false

        return true
    }

    override fun hashCode(): Int {
        var result = fuelTypeId.hashCode()
        result = 31 * result + (servicesIds?.hashCode() ?: 0)
        result = 31 * result + (stationChainsIds?.hashCode() ?: 0)
        result = 31 * result + (minPrice?.hashCode() ?: 0)
        result = 31 * result + (maxPrice?.hashCode() ?: 0)
        result = 31 * result + (userLongitude?.hashCode() ?: 0)
        result = 31 * result + (userLatitude?.hashCode() ?: 0)
        result = 31 * result + (distance?.hashCode() ?: 0)
        return result
    }
}