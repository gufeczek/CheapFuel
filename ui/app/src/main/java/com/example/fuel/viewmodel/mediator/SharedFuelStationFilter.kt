package com.example.fuel.viewmodel.mediator

object SharedFuelStationFilter {
    var fuelTypeId: Long? = null

    fun setFuelTypeId(fuelTypeId: Long) {
        this.fuelTypeId = fuelTypeId
    }

    fun isNotEmpty(): Boolean {
        return this.fuelTypeId != null
    }

    fun clear() {
        this.fuelTypeId = null
    }
}