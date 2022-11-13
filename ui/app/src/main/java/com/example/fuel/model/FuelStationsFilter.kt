package com.example.fuel.model

class FuelStationsFilter(
    var fuelTypeId: Long,
    var servicesIds: MutableList<Long>?,
    var stationChainsIds: MutableList<Long>?,
    var minPrice: Double?,
    var maxPrice: Double?
) {

    fun copy(): FuelStationsFilter {
        return FuelStationsFilter(
            this.fuelTypeId,
            servicesIds?.toMutableList(),
            stationChainsIds?.toMutableList(),
            minPrice,
            maxPrice
        )
    }

    override fun equals(other: Any?): Boolean {
        if (this === other) return true
        if (javaClass != other?.javaClass) return false

        other as FuelStationsFilter

        if (fuelTypeId != other.fuelTypeId) return false
        if (servicesIds != other.servicesIds) return false
        if (stationChainsIds != other.stationChainsIds) return false
        if (minPrice != other.minPrice) return false
        if (maxPrice != other.maxPrice) return false

        return true
    }

    override fun hashCode(): Int {
        var result = fuelTypeId.hashCode()
        result = 31 * result + (servicesIds?.hashCode() ?: 0)
        result = 31 * result + (stationChainsIds?.hashCode() ?: 0)
        result = 31 * result + (minPrice?.hashCode() ?: 0)
        result = 31 * result + (maxPrice?.hashCode() ?: 0)
        return result
    }
}