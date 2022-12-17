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
    }

    fun fuelStationChanged() {
        changeDetected = true
    }

    fun act() {
        if (changeDetected) {
            mapViewModel!!.hardReload()
            mapViewModel!!.getFuelStations()
            mapViewModel!!.clearHardReload()
        }
    }
}