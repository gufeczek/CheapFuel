package com.example.fuel.viewmodel

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.fuel.model.SimpleMapFuelStation
import com.example.fuel.repository.FuelStationRepository
import kotlinx.coroutines.launch
import retrofit2.Response

class FuelStationMapViewModel(
    private val fuelStationRepository: FuelStationRepository): ViewModel() {
        val fuelStations: MutableLiveData<Response<Array<SimpleMapFuelStation>>> = MutableLiveData()

        fun getFuelStations(fuelTypeId: Long) {
            viewModelScope.launch {
                fuelStations.value = fuelStationRepository.getSimpleMapFuelStations(fuelTypeId)
            }
        }
}