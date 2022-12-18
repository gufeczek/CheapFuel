package com.example.fuel.viewmodel.mediator

import com.example.fuel.viewmodel.FuelStationMapViewModel

object MapViewModelMediator {
    private var mapViewModel: FuelStationMapViewModel? = null
    private var changeDetected = false

    fun subscribe(mapViewModel: FuelStationMapViewModel) {
        this.mapViewModel = mapViewModel
    }

    fun unsubscribe() {
        this.mapViewModel = null
        changeDetected = false
    }

    fun fuelStationChanged() {
        changeDetected = true
    }

    fun act() {
        if (changeDetected && mapViewModel != null) {
            mapViewModel!!.hardReload()
            mapViewModel!!.getFuelStations()
            mapViewModel!!.clearHardReload()
        }
    }
}