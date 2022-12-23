package com.example.fuel.viewmodel.mediator

import com.example.fuel.viewmodel.FuelStationListViewModel

object ListViewModelMediator {
    private var listViewModel: FuelStationListViewModel? = null
    private var changeDetected = false

    fun subscribe(listViewModel: FuelStationListViewModel) {
        this.listViewModel = listViewModel
    }

    fun unsubscribe() {
        this.listViewModel = null
        changeDetected = false
    }

    fun fuelStationChanged() {
        changeDetected = true
    }

    fun act() {
        if (changeDetected && listViewModel != null) {
            listViewModel!!.hardReload()
            listViewModel!!.getFirstPageOfFuelStations()
        }
    }
}